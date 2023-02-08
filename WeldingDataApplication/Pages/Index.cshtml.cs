using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeldingDataApplication.Classes;

namespace WeldingDataApplication.Pages
{
    public class IndexModel : PageModel
    {
        static HttpClient myHttpClient = new HttpClient();
        public List<Weld.WeldInfo>? Message;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            Message = await myHttpClient.GetFromJsonAsync<List<Weld.WeldInfo>>("http://weldcube.ky.local/api/v4/Welds?api_key=dc55e8bbc6b73dbb17c5ecf360a0aeb1%20");
        }

        public void OnGet()
        {
            
        }
    }
}