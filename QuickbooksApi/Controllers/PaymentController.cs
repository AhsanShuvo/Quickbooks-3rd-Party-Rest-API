using Newtonsoft.Json;
using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using QuickbooksApi.Models;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuickbooksApi.Controllers
{
    public class PaymentController : BaseController
    {
        private IPaymentRepository _repository;

        public PaymentController(
            IPaymentRepository repository, IApiDataProvider provider,
            IJsonToModelBuilder builder): base(provider, builder)
        {
            _repository = repository;
        }

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
            Logger.WriteDebug("Creating new payment.");
            PaymentInfo payment = new PaymentInfo()
            {
                TotalAmt = model.TotalAmt,
                CustomerRef = model.CustomerRef
            };
            var requestBody = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");
            var paymentObj= await HandlePostRequest(requestBody, "payment");
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
            await HandleDeleteRequest(requestBody, EntityType.Payment.ToString().ToLower());
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