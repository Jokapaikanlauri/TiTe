using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Mail;
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

        //lokitiedot
        DateTime aika = DateTime.Now;
        public List<string> errorloki = new List<string>();
        public List<string> onnistumisloki = new List<string>();


        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        //Sähköpostin lähetyksenkoodi, ei vielä testattu. Koitan kommentoida jotta muistaa jatkossa mitä on ajatellu
        public async Task ErrorMessage()
        {
            //onnistumisloki.Add("Sähköpostin lähetys aloitettu");
            //tätä funktiota kutsutaan kun eri funktiossa, fetsauksen jälkeen tälle funktiolla välitetään haettu tieto, ja tarkistellaan 
            //onko error tapahtunut. Lisätään vastaan otettava OBJEKTI myöhemmin, tai muokataan koodia, ottamaan vastaan muuttuja.

            //TÄMÄ POIS KUN NOUTOKOODAUS ON VALMIS.
            object WeldinError = null;

            //Jos 
            if (WeldinError == null)
            {
                // Luodaan sähköpostiviesti. Päätetään myöhemmin millainen rakenne.
                var message = new MailMessage();
                //Valitaan vastaanottajat
                message.To.Add(new MailAddress("simo.hamalainen@edu.savonia.fi"));
                //Valitaan lähettäjä
                message.From = new MailAddress("savoniankumipojat@gmail.com");
                //Viestin otsikko
                message.Subject = "Virhehitsauksessa: " + "TÄHÄN TIETO VÄLITETYSTÄ MUUTUJASTA";
                //Viesti, tehdäänkä html elementtinä, vaiko vain viestinä???                          
                message.Body = "Tapahtui virhe: " + "TÄHÄN VIESTI";





                try
                {
                    // SMTP asetukset. Loin Gmailiin postilaatikon. Tosi turvallista tämmönen kovakoodaus :D mutta koska Savonia.
                    var smtpClient = new SmtpClient("smtp.gmail.com", 587);
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential("savoniankumipojat@gmail.com", "Salasana12345");
                    // Lähetetään sähköpostiviesti, jos luoja suo
                    smtpClient.Send(message);
                    onnistumisloki.Add(aika + " :Lähetetään virheilmoitus sähköpostilla: " + message);
                }
                catch (Exception ex)
                {
                    errorloki.Add(aika + ": Sähköpostipalvelin ei vastaa: " + ex);
                    errorloki.Add(aika + ": Sähköpostin lähettäminen epäonnistui: " + message);

                }
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
                    }

                }
                // Suljetaan kirjottaminen
                sw.Close();

            }
            catch (Exception e)
            {
                errorloki.Add(aika + ": Yhteys Savoniaan ei onnistu: " + e);
                ErrorMessage();
            }
        }


    }
}