using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        
       

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            // Muutetaan haettu json fomraatttin
            myHttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //Message = await myHttpClient.GetFromJsonAsync<List<Weld.WeldInfo>>("http://weldcube.ky.local/api/v4/Welds?api_key=dc55e8bbc6b73dbb17c5ecf360a0aeb1%20");


            // Haetaan rajapinnasta 
             Message = await myHttpClient.GetFromJsonAsync<Weld.Root>("http://weldcube.ky.local/api/v4/Welds?api_key=dc55e8bbc6b73dbb17c5ecf360a0aeb1%20");
            //Message = await myHttpClient.GetFromJsonAsync<Weld.Root>("http://weldcube.ky.local/api/v4/Welds?api_key=dc55e8bbc6b73dbb17c5ecf360a0aeb1");
            Console.WriteLine("Olen Message " + Message);


            // Alotetaan looppaamaan kaikki hitsaukset
             foreach (Weld.WeldInfo item in  Message.WeldInfos)
            {
                // Luodaan url + api key, että saadaan rajapinnan tieto ulos
               
                Console.WriteLine("");

                Console.WriteLine("Id: "+item.Id);
                Console.WriteLine("State: " + item.State);
                Console.WriteLine("ProcessingStepNumber: " + item.ProcessingStepNumber);
                Console.WriteLine("MachineSerialNumber: " + item.MachineSerialNumber);
                Console.WriteLine("PartSerialNumber: " + item.PartSerialNumber);
                Console.WriteLine("MachineType: " + item.MachineType);
                Console.WriteLine("Details: " + item.Details);
                var url = item.Details;
                url += apiKey;
               // rootList = await myHttpClient.GetFromJsonAsync<WeldDetails>(item.Details+apiKey);
                Console.WriteLine("Message limitviolation: "+Message);

                Console.WriteLine("Welder: " + item.Welder);
                Console.WriteLine("Timestamp: " + item.Timestamp);
                Console.WriteLine("");

  
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

                //    //    Console.WriteLine("ERROR: " + kusi.ValueType + " AND" + kusi.ViolationType);

                //    //}
                //}
            }

        }


    }
}