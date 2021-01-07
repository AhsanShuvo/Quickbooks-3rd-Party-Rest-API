using QuickbooksAPI.Interfaces;
using QuickbooksCommon.Logger;
using QuickbooksDAL;
using QuickbooksDAL.Interfaces;
using Newtonsoft.Json;
using QuickbooksWeb.Interfaces;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuickbooksWeb.Controllers
{
    public class InvoiceController : BaseController
    {
        private IInvoiceRepository _repository;

        public InvoiceController(
            IInvoiceRepository repository, IApiDataProvider provider,
            IJsonToModelBuilder builder, IApiModelToEntityModelBuilder entityBuilder): base(provider, builder, entityBuilder)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateInvoice()
        {
            return View();
        }

        public ActionResult DeleteInvoice()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> DeleteInvoice(string id)
        {
            Logger.WriteDebug("Deleting invoice.");
            InvoiceInfo invoice = new InvoiceInfo()
            {
                Id = id,
                SyncToken = "0"
            };

            var requestBody = new StringContent(JsonConvert.SerializeObject(invoice), Encoding.UTF8, "application/json");
            await HandleDeleteRequest(requestBody, EntityType.Invoice.ToString().ToLower());
            _repository.DeleteInvoiceInfo(id);

            return RedirectToAction("Index");
        }

        public ActionResult DownloadInvoice()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> DownloadInvoice(string id)
        {
            Logger.WriteDebug("Downloading invoice.");
            var invoiceId = WebUtility.UrlEncode(id);
            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/invoice/{2}/pdf?minorversion=55", qboBaseUrl, realmId, invoiceId);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            await _provider.GetPDF(uri, token);

            return RedirectToAction("Index");
        }
    }
}