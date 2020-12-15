using Newtonsoft.Json;
using QuickbooksApi.ApiService;
using QuickbooksApi.ModelBuilder;
using QuickbooksApi.Models;
using QuickbooksApi.Repository;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuickbooksApi.Controllers
{
    public class PaymentController : Controller
    {
        ApiDataProvider _provider = new ApiDataProvider();
        JsonToModelBuilder _builder = new JsonToModelBuilder();
        PaymentRepository _repository = new PaymentRepository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreatePayment()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreatePayment(PaymentInfo model)
        {
            PaymentInfo payment = new PaymentInfo()
            {
                TotalAmt = model.TotalAmt,
                CustomerRef = model.CustomerRef
            };

            var requestBody = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");

            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/payment?minorversion=55", qboBaseUrl, realmId);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            var paymentObj= await _provider.Post(uri, requestBody, token);
            var paymentInfo = _builder.GetPaymentModel(paymentObj);
            _repository.SavePaymentInfo(paymentInfo);
            return RedirectToAction("Index");
        }

        public ActionResult DeletePayment()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> DeletePayment(string id)
        {

            PaymentInfo paymentData = _repository.GetPaymentInfo(id);
            PaymentInfo payment = new PaymentInfo()
            {
                Id = id,
                SyncToken = paymentData.SyncToken
            };

            var requestBody = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");

            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/payment?operation=delete&minorversion=55", qboBaseUrl, realmId);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            var deleteStatus = await _provider.Post(uri, requestBody, token);
            _repository.DeletePayment(id);

            return RedirectToAction("Index");
        }

        public ActionResult DownloadPayment()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> DownloadPayment(string id)
        {
            var paymentId = WebUtility.UrlEncode("155");
            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/payment/{2}/pdf?minorversion=55", qboBaseUrl, realmId, paymentId);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            await _provider.GetPDF(uri, token);

            return RedirectToAction("Index");
        }
    }
}