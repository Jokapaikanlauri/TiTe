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
            onnistumisloki.Add(aika + ": Ohjelma käynnistyi");
            try
            {
                //Etisivulla oleva otsikko kertoo, ollaanko savoniassa vai kotona.

                //errorloki.Add(localDate + ": Yhdistetään Savonian verkoon");
                // Muutetaan haettu json fomraatttin
                myHttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //Message = await myHttpClient.GetFromJsonAsync<List<Weld.WeldInfo>>("http://weldcube.ky.local/api/v4/Welds?api_key=dc55e8bbc6b73dbb17c5ecf360a0aeb1%20");


                // Haetaan rajapinnasta 
                Message = await myHttpClient.GetFromJsonAsync<Weld.Root>("http://weldcube.ky.local/api/v4/Welds?api_key=dc55e8bbc6b73dbb17c5ecf360a0aeb1%20");
                //Message = await myHttpClient.GetFromJsonAsync<Weld.Root>("http://weldcube.ky.local/api/v4/Welds?api_key=dc55e8bbc6b73dbb17c5ecf360a0aeb1");
                Console.WriteLine("Olen Message " + Message);


                // Alotetaan looppaamaan kaikki hitsaukset
                foreach (Weld.WeldInfo item in Message.WeldInfos)
                {
                    // Luodaan url + api key, että saadaan rajapinnan tieto ulos

                    Console.WriteLine("");

                    Console.WriteLine("Id: " + item.Id);
                    Console.WriteLine("State: " + item.State);
                    Console.WriteLine("ProcessingStepNumber: " + item.ProcessingStepNumber);
                    Console.WriteLine("MachineSerialNumber: " + item.MachineSerialNumber);
                    Console.WriteLine("PartSerialNumber: " + item.PartSerialNumber);
                    Console.WriteLine("MachineType: " + item.MachineType);
                    Console.WriteLine("Details: " + item.Details);
                    var url = item.Details;
                    url += apiKey;
                    // rootList = await myHttpClient.GetFromJsonAsync<WeldDetails>(item.Details+apiKey);
                    Console.WriteLine("Message limitviolation: " + Message);

                    Console.WriteLine("Welder: " + item.Welder);
                    Console.WriteLine("Timestamp: " + item.Timestamp);
                    Console.WriteLine("");

                    //Frontendin lokit.




                    // Pitäisi saada gitsaus objecti ulo ,että voidaan eritellä haluamat tiedot.
                    var rootWanted = await myHttpClient.GetFromJsonAsync<WeldDetails>(url);
                    Console.WriteLine(rootWanted);


                    //Console.WriteLine(rootWanted);//vittu to string
                    //rootList.Add();
                    //Thread.Sleep(1000);

                    // TÄ ÄEI VIELÄ TOIMI!
                    //foreach (WeldDetails.WeldDataLimitViolation virhe in rootWanted)
                    //{
                    //    Console.WriteLine(virhe);
                    //    //foreach (WeldDetails.LimitViolation kusi in paska.LimitViolations)
                    //    //{

                    //    //    Console.WriteLine("" + kusi.ValueType + " AND" + kusi.ViolationType);

                    //    //}
                    //}
                }

            }
            catch (Exception e)
            {
                errorloki.Add(aika + ": Yhteys Savoniaan ei onnistu: " + e);
                ErrorMessage();
            }
        }


    }
}