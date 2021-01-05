using Newtonsoft.Json;
using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using QuickbooksApi.Models;
using System;
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
            IJsonToModelBuilder builder, IApiModelToEntityModelBuilder entityBuilder): base(provider, builder, entityBuilder)
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
        public async Task<ActionResult> CreatePayment(PaymentModel model)
        {
            Logger.WriteDebug("Creating a new payment.");
            var requestBody = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var paymentObj= await HandlePostRequest(requestBody, "payment");
            var paymentModel = _builder.GetPaymentModel(paymentObj);
            var paymentEntityModel = _entityBuilder.GetPaymentEntityModel(paymentModel);
            _repository.SavePaymentInfo(paymentEntityModel);
            return RedirectToAction("Index");
        }

        public ActionResult DeletePayment()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> DeletePayment(string id)
        {
            Logger.WriteDebug("Deleting a payment.");
            PaymentInfo paymentData = _repository.GetPaymentInfo(id);
            PaymentModel payment = new PaymentModel()
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
            var paymentId = WebUtility.UrlEncode(id);
            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/payment/{2}/pdf?minorversion=55", qboBaseUrl, realmId, paymentId);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            try
            {
                await _provider.GetPDF(uri, token);
            }
            catch(Exception e)
            {
                Logger.WriteError(e, "Failed to download payment.");
            }

            return RedirectToAction("Index");
        }
    }
}