using Newtonsoft.Json;
using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using QuickbooksApi.Models;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuickbooksApi.Controllers
{
    public class CustomerController : BaseController
    {
        private ICustomerRepository _repository;

        public CustomerController(
            ICustomerRepository repository, IApiDataProvider provider,
            IJsonToModelBuilder builder): base(provider, builder)
        {
            _repository = repository;
        }

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
            Logger.WriteDebug("Creating new customer");
            var customer = new CustomerInfo {
                DisplayName =model.DisplayName,
                CompanyName = model.CompanyName,
                Balance = model.Balance,
                Active = model.Active
            };
            var requestBody = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            var customerObject = await HandlePostRequest(requestBody, EntityType.Customer.ToString().ToLower());
            CustomerInfo customerInfo = _builder.GetCustomerModel(customerObject);
            _repository.SaveCustomerDetails(customerInfo);
            return RedirectToRoute("Index");
        }

        public async Task<ActionResult> CustomerDetails()
        {
            var customerObject = await HandleGetRequest("68", EntityType.Customer.ToString().ToLower());
            var customerInfo = _builder.GetCustomerModel(customerObject);
            _repository.SaveCustomerDetails(customerInfo);
            return View(customerInfo);
        }

        public async Task<ActionResult> UpdateCustomer()
        {
            var customer = new CustomerInfo
            {
                CompanyName = "New Aviation 2",
                DisplayName = "Mark",
                SyncToken = "0",
                Id = "2",
            };

            var requestBody = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            var customerObj = await HandlePostRequest(requestBody, EntityType.Customer.ToString().ToLower());
            var customerInfo = _builder.GetCustomerModel(customerObj);
            _repository.SaveCustomerDetails(customerInfo);
            return RedirectToAction("Index");
        }
    }
}