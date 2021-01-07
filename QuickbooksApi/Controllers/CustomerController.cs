using QuickbooksAPI.Interfaces;
using QuickbooksCommon.Logger;
using QuickbooksDAL;
using QuickbooksDAL.Interfaces;
using Newtonsoft.Json;
using QuickbooksWeb.Interfaces;
using QuickbooksWeb.Models;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuickbooksWeb.Controllers
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
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> CustomerDetails()
        {
            var customerObject = await HandleGetRequest("68", EntityType.Customer.ToString().ToLower());
            var customerModel = _builder.GetCustomerModel(customerObject);
            var customerEntityModel = _entityBuilder.GetCustomerEntityModel(customerModel);
            _repository.SaveCustomerDetails(customerEntityModel);
            return View(customerModel);
        }

        public ActionResult UpdateCustomer()
        {
            var model = _repository.GetCustomerInfo("2");
            return View(model);
        }

        public async Task<ActionResult> UpdateCustomer(CustomerInfo model)
        {
            var requestBody = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var customerObj = await HandlePostRequest(requestBody, EntityType.Customer.ToString().ToLower());
            var customerModel = _builder.GetCustomerModel(customerObj);
            var customerEntityModel = _entityBuilder.GetCustomerEntityModel(customerModel);
            _repository.SaveCustomerDetails(customerEntityModel);
            return RedirectToAction("Index");
        }
    }
}