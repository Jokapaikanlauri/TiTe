using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Mail;
using System.Text;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeldingDataApplication.Classes;

namespace WeldingDataApplication.Pages
{
    public class IndexModel : PageModel
    {
        static HttpClient myHttpClient = new HttpClient();
        public Weld.Root? Message;
        public List<WeldDetails.WeldData>? rootList;
        public WeldDetails.RootObject rootElement;
        private readonly ILogger<IndexModel> _logger;
        private string apiKey = "?api_key=dc55e8bbc6b73dbb17c5ecf360a0aeb1";

        
        static string apiEndpoint = "https://api.courier.com/send";
        static string token = "Bearer " + "dk_prod_0N0HT7KZK64JPHHD1CXY0PKQBF5K";

        //lokitiedot
        DateTime aika = DateTime.Now;
        public List<string> errorloki = new List<string>();
        public List<string> onnistumisloki = new List<string>();

        public string sample = "tämä on testiteksti";

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        //Sähköpostin lähetyksenkoodi, ei vielä testattu. Koitan kommentoida jotta muistaa jatkossa mitä on ajatellu


        public async Task sendEmail()
        {
            try { 
            _logger.LogError("sendemail alku");
            // attach the Auth Token
            myHttpClient.DefaultRequestHeaders.Add("Authorization", token);

            // Data.variablesiin pitää viitata JSON-taulukon kautta jotta voidaan ilmoittaa muuttuja sähköpostitse
            string payload =
            "{ \"message\": {\"to\": {\"email\": \"savoniankumipojat@gmail.com\"},\"template\": \"BVEPX0PS6D4JK5G85C612850QTEB\", \"data\": {\"variables\":\"sample\"}}}";


            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

            var resp = await myHttpClient.PostAsync(new Uri(apiEndpoint), content);
            Console.WriteLine(resp);
            }
            catch (Exception e)
            {
                errorloki.Add("Loppuko pojilta kumit");
            }
        }

        public async Task OnGet()
        {
            //onnistumisloki.Add(aika + ": Ohjelma käynnistyi");
            try
            {
                //Etisivulla oleva otsikko kertoo, ollaanko savoniassa vai kotona.

                //errorloki.Add(localDate + ": Yhdistetään Savonian verkoon");
                // Muutetaan haettu json formaatttin
                myHttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //Message = await myHttpClient.GetFromJsonAsync<List<Weld.WeldInfo>>("http://weldcube.ky.local/api/v4/Welds?api_key=dc55e8bbc6b73dbb17c5ecf360a0aeb1%20");


                // Haetaan rajapinnasta 
                Message = await myHttpClient.GetFromJsonAsync<Weld.Root>("http://weldcube.ky.local/api/v4/Welds?api_key=dc55e8bbc6b73dbb17c5ecf360a0aeb1%20");
                //Message = await myHttpClient.GetFromJsonAsync<Weld.Root>("http://weldcube.ky.local/api/v4/Welds?api_key=dc55e8bbc6b73dbb17c5ecf360a0aeb1");
                Console.WriteLine("Olen Message " + Message);

                // Alotetaan looppaamaan kaikki hitsaukset
                var i = 0;
                var o = 0;
                StreamWriter sw = new StreamWriter("./WeldingDataApplication/AllWelds.txt");


                foreach (Weld.WeldInfo item in Message.WeldInfos)
                {

                    var url = item.Details;
                    url += apiKey;
                    // Kirjoitetaan txt tiedostoon kaikki nyt olemassa olevat tiedostot
                    // sw.WriteLine(i + ": Hitsaus id: " + item.Id + " State: " + item.State + " TimeStamp: " + item.Timestamp);

                    /*   //Pass the file path and file name to the StreamReader constructor
                        StreamReader sr = new StreamReader("C:\\Sample.txt");
                        //Read the first line of text
                        line = sr.ReadLine();
                        //Continue to read until you reach end of file
                        while (line != null)
                        {
                            //write the line to console window
                            Console.WriteLine(line);
                            //Read the next line
                            line = sr.ReadLine();
                        }
                        //close the file
                        sr.Close();*/

                    if (item.State != "NotOk")
                    {

                        onnistumisloki.Add(i + ": Hitsaus id: " + item.Id + " State: " + item.State + " TimeStamp: " + item.Timestamp);
                        i++;
                    }

                    //Frontendin lokit.

                    var rootWanted = await myHttpClient.GetFromJsonAsync<WeldDetails.RootObject>(url);

                    // täältä tulee hits id jolla voi olla paaljon erroreita

                    foreach (var violation in rootWanted.WeldData.LimitViolations)
                    {
                        string valueType = violation.ValueType;
                        string violationType = violation.ViolationType;
                        // Jos sama id nii ei luoda id:lle uutta rivia vaan lisätään id:hen violation objectit
                        errorloki.Add(o + ": Hitsaus id: " + item.Id + " Valuetype: " + violation.ValueType + " Violationtype: " + violation.ViolationType + " Status: " + item.State + " TimeStap: " + item.Timestamp);
                        o++;
                        //await sendEmail();
                    }

                }
                // Suljetaan kirjottaminen
                sw.Close();

            }
            catch (Exception e)
            {
                errorloki.Add(aika + ": Yhteys Savoniaan ei onnistu: " + e);
                await sendEmail();
            }
        }


    }
}