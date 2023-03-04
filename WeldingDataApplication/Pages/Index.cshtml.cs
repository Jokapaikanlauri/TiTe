using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Mail;
using System.Text;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeldingDataApplication.Classes;
using System.Text.Json;
using System.Net.NetworkInformation;
using System.Text.Json.Serialization;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;



namespace WeldingDataApplication.Pages
{
    public class IndexModel : PageModel
    {
        static HttpClient myHttpClient = new HttpClient();
        public Weld.Root? Message;
        public List<WeldDetails.RootObject>? rootList;

        public List<Database>? DatabaseItems = new List<Database>();
        public List<Database>? SuccessList = new List<Database>();
        public List<Database>? ErrorsList = new List<Database>();
        private readonly ILogger<IndexModel> _logger;
        private string apiKey = "?api_key=dc55e8bbc6b73dbb17c5ecf360a0aeb1";
        public string filePath = "./WeldData.json";

        static string apiEndpoint = "https://api.courier.com/send";
        static string token = "Bearer " + "dk_prod_0N0HT7KZK64JPHHD1CXY0PKQBF5K";
        

        //lokitiedot
        DateTime aika = DateTime.Now;
        DateTime dbAika = DateTime.Now;
        public List<string> errorloki = new List<string>();
        public List<string> onnistumisloki = new List<string>();

        public string sample = "tämä on testiteksti";

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            
        }
      
        public static void addDataBase(object data)
        {
           // Databasen itemien lisäys tähän?
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
                //errorloki.Add(localDate + ": Yhdistetään Savonian verkoon");
                // Muutetaan haettu json formaatttin
                myHttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                // Haetaan rajapinnasta 
                Message = await myHttpClient.GetFromJsonAsync<Weld.Root>("http://weldcube.ky.local/api/v4/Welds?api_key=dc55e8bbc6b73dbb17c5ecf360a0aeb1%20");
                var i = 0;
                var o = 0;
                var a = 0;

                string WeldData =  System.IO.File.ReadAllText("./WeldData.json");
                List <Database> data = JsonConvert.DeserializeObject<List<Database>>(WeldData);

                if(data != null)
                {
                    foreach (var weldItem in data)
                    {
                       
                        // Console.WriteLine(weldItem.Row);
                    }
                }

                foreach (Weld.WeldInfo item in Message.WeldInfos)
                {
                    var url = item.Details;
                    url += apiKey;
                    dbAika = item.Timestamp;
                    var wantedTime = aika - dbAika;

                    //Otetaan item kiinni jossa on errori, ja tarkastetaan sen errorit
                    if (item.State != "Ok")
                    {
                        // Haetaan detailit vain jossa on error olemassa
                        var rootWanted = await myHttpClient.GetFromJsonAsync<WeldDetails.RootObject>(url);

                        foreach (var violation in rootWanted.WeldData.LimitViolations)
                        {
                            string valueType = violation.ValueType;
                            string violationType = violation.ViolationType;
                      
                 
                            o++;
                            // Hitsaukset jossa on error
                            // Tarkastetaan onko id jo listassa, ennen sen lisäystä. // TODO tämä ei vielä relevantti
                            // Jos Tieto on liian vanhaa, ei sitä oteta db listaan mukaan
                            if (wantedTime.Days < 17)
                            {
                                ErrorsList.Add(new Database
                                {
                                    Row = o,
                                    Id = item.Id,
                                    ViolationType = violation.ViolationType,
                                    Valuetype = violation.ValueType,
                                    State = item.State,
                                    Time = item.Timestamp.ToShortTimeString(),
                                    Date = item.Timestamp.ToShortDateString(),
                                    TimeStamp = item.Timestamp,
                                    PartItemNumber = rootWanted.PartItemNumber,
                                    PartSerialNumber = rootWanted.PartSerialNumber,
                                   
                                });
                                await sendEmail();
                                // Päivitetään tiedot
                                // Tämä alla oleva saatava toimimaan funtion kautta
                                a++;
                                DatabaseItems.Add(new Database
                                {
                                    Row = a,
                                    Id = item.Id,
                                    PartItemNumber = rootWanted.PartItemNumber,
                                    PartSerialNumber = rootWanted.PartSerialNumber,
                                    State = item.State,
                                    TimeStamp = item.Timestamp,
                                    Time = item.Timestamp.ToShortTimeString(),
                                    Date = item.Timestamp.ToShortDateString(),
                                    ViolationType = violation.ViolationType,
                                    Valuetype = violation.ValueType
                                });

                            }
                                                  
                        }
                    }
                    // Tästä thtävä oma funktio, jota voi kutsua
                    // error ja succsess listan teon yhteydessä
                    // Jos Tieto on liian vanhaa, ei sitä oteta dblistaan mukaan
                    //if(wantedTime.Days < 17  )
                    //{
                    //    a++;
                    //    DatabaseItems.Add(new Database
                    //    {
                    //        Row = a,
                    //        Id = item.Id,
                    //        State = item.State,
                    //        TimeStamp = item.Timestamp,
                    //        Time = item.Timestamp.ToShortTimeString(),
                    //        Date = item.Timestamp.ToShortDateString(),
                    //    });
                    //}

                    // Tarkastetaan onnistuneiden hitsausten määrä viimeiseltä 5:ltä päivältä
                    if (item.State != "NotOk" && wantedTime.Days <= 5)
                    {
                        i++;
                        // Onnistuneet hitsaukset lista
                        SuccessList.Add(new Database
                        {
                            Row = i,
                            Id = item.Id,
                            State = item.State,
                            Time = item.Timestamp.ToShortTimeString(),
                            Date = item.Timestamp.ToShortDateString(),
                            TimeStamp = item.Timestamp
                        });
                        a++;
                        DatabaseItems.Add(new Database
                        {
                            Row = a,
                            Id = item.Id,
                            State = item.State,
                            TimeStamp = item.Timestamp,
                            Time = item.Timestamp.ToShortTimeString(),
                            Date = item.Timestamp.ToShortDateString(),
                        });

                    }

                }
                
                // Kirjoitetaan jsion 
                string jsonString = JsonConvert.SerializeObject(DatabaseItems, Formatting.Indented);
                byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
                System.IO.File.WriteAllBytes(filePath, bytes);
       
          
            }
            catch (Exception e)
            {
                errorloki.Add(aika + ": Yhteys Savoniaan ei onnistu: " + e);
                await sendEmail();
            }
        }
    }
}