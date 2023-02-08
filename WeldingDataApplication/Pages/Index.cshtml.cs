using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WeldingDataApplication.Pages
{
    public class IndexModel : PageModel
    {
        static HttpClient myHttpClient = new HttpClient();
        public string Message { get; set; }
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            Message = myHttpClient.GetAsync("http://weldcube.ky.local/api/v4/Welds?api_key=dc55e8bbc6b73dbb17c5ecf360a0aeb1%20").ToString();

        }
        public void OnGet()
        {
            
        }
    }
}