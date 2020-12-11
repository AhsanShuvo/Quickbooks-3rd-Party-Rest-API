using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace QuickbooksApi.Controllers
{
    public class WebhookController : Controller
    {
        private string _signature = ConfigurationManager.AppSettings["intuit-signature"];
        private string _algorithm = ConfigurationManager.AppSettings["algorithm"];

        [HttpPost]
        public ActionResult Index()
        {
            var headers = Request.Headers;
            if (Request.Headers.AllKeys.Contains("Intuit-Signature"))
            {
                var value = Request.Headers["Intuit-Signature"];
            }
            var payload = Request.RequestType;
            var code = Request.Files.AllKeys;
            return View();
        }
    }
}