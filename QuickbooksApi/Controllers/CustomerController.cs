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
            IJsonToModelBuilder builder, IApiModelToEntityModelBuilder entityBuilder): base(provider, builder, entityBuilder)
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
        public async Task<ActionResult> CreateCustomer(CustomerModel model)
        {
            Logger.WriteDebug("Creating new customer");
            var requestBody = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var customerObject = await HandlePostRequest(requestBody, EntityType.Customer.ToString().ToLower());
            CustomerModel customerModel = _builder.GetCustomerModel(customerObject);
            CustomerInfo customerEntityModel = _entityBuilder.GetCustomerEntityModel(customerModel);
            _repository.SaveCustomerDetails(customerEntityModel);
            return RedirectToRoute("Index");
        }

        public async Task<ActionResult> CustomerDetails()
        {
            var customerObject = await HandleGetRequest("68", EntityType.Customer.ToString().ToLower());
            var customerModel = _builder.GetCustomerModel(customerObject);
            var customerEntityModel = _entityBuilder.GetCustomerEntityModel(customerModel);
            _repository.SaveCustomerDetails(customerEntityModel);
            return View(customerModel);
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
            var customerModel = _builder.GetCustomerModel(customerObj);
            var customerEntityModel = _entityBuilder.GetCustomerEntityModel(customerModel);
            _repository.SaveCustomerDetails(customerEntityModel);
            return RedirectToAction("Index");
        }
    }
}