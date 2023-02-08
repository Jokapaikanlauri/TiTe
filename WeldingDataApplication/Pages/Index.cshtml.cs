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
        public WeldDetails.Root rootElement;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            myHttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //Message = await myHttpClient.GetFromJsonAsync<List<Weld.WeldInfo>>("http://weldcube.ky.local/api/v4/Welds?api_key=dc55e8bbc6b73dbb17c5ecf360a0aeb1%20");
            Message = await myHttpClient.GetFromJsonAsync<Weld.Root>("http://weldcube.ky.local/api/v4/Welds?api_key=dc55e8bbc6b73dbb17c5ecf360a0aeb1%20");
            foreach(Weld.WeldInfo item in Message.WeldInfos)
            {
                var url = item.Details;              
                rootElement = await myHttpClient.GetFromJsonAsync<WeldDetails.Root>(url);
                rootList.Add(rootElement.WeldData);
                Thread.Sleep(1000);

                foreach(WeldDetails.WeldData paska in rootList)
                {
                    foreach(WeldDetails.LimitViolation kusi in paska.LimitViolations)
                    {

                        Console.WriteLine("ERROR: " + kusi.ValueType + " AND" + kusi.ViolationType);

                    }
                }
            }
            
        }


    }
}