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
    public class CustomerController : Controller
    {
        private JsonToModelBuilder _builder = new JsonToModelBuilder();
        private CustomerRepository _repository = new CustomerRepository();
        private ApiDataProvider _provider = new ApiDataProvider();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateCustomer(CustomerInfo model)
        {
            var customer = new CustomerInfo {
                DisplayName =model.DisplayName,
                CompanyName = model.CompanyName,
                Balance = model.Balance,
                Active = model.Active
            };
            var requestBody = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");

            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/customer?minorversion=55", qboBaseUrl, realmId);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            var customerObject = await _provider.Post(uri, requestBody, token);
            CustomerInfo customerInfo = _builder.GetCustomerModel(customerObject);
            _repository.SaveCustomerDetails(customerInfo);
            return RedirectToRoute("Index");
        }

        public async Task<ActionResult> CustomerDetails()
        {
            var customerId = "68";
            var id = WebUtility.UrlEncode(customerId);
            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/customer/{2}?minorversion=55", qboBaseUrl, realmId, id);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            var customerObject = await _provider.Get(uri, token);
            var customerInfo = _builder.GetCustomerModel(customerObject);
            _repository.SaveCustomerDetails(customerInfo);
            return View(customerInfo);
        }

        public async Task<ActionResult> UpdateCustomer()
        {
            var customer = new CustomerInfo
            {
                CompanyName = "New Aviation",
                DisplayName = "Mark",
                SyncToken = "0",
                Id = "2",
            };

            var requestBody = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");

            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/customer?minorversion=55", qboBaseUrl, realmId);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            var customerObj = await _provider.Post(uri, requestBody, token);
            var customerInfo = _builder.GetCustomerModel(customerObj);
            _repository.SaveCustomerDetails(customerInfo);
            return RedirectToAction("Index");
        }
    }
}