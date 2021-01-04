using QuickbooksApi.Interfaces;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuickbooksApi.Controllers
{
    public class WebhookController : Controller
    {
        private IWebhookManager _manager;

        public WebhookController(IWebhookManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        public async Task<ActionResult> Index()
        {
            var req = Request.InputStream;
            var json = new StreamReader(req).ReadToEnd();
            var signature = Request.Headers["Intuit-Signature"];
            await _manager.Init(json, signature);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}