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
    public class EmployeeController : BaseController
    {
        private IEmployeeRepository _repository;

        public EmployeeController(
            IEmployeeRepository repository, IApiDataProvider provider,
            IJsonToModelBuilder builder, IApiModelToEntityModelBuilder entityBuilder): base(provider, builder, entityBuilder)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateEmployee(EmployeeModel model)
        {
            Logger.WriteDebug("Creating new employee");
            var requestBody = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var employeeObject = await HandlePostRequest(requestBody, EntityType.Employee.ToString().ToLower());
            var employeeModel = _builder.GetEmployeeModel(employeeObject);
            var employeeEntityModel = _entityBuilder.GetEmployeeEntityModel(employeeModel);
            _repository.SaveEmployeeInfo(employeeEntityModel);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> EmployeeDetails()
        {
            Logger.WriteDebug("Getting employee details from quickbooks online.");
            var employeeId = "77";
            var employeeObject = await HandleGetRequest(employeeId, EntityType.Employee.ToString().ToLower());
            var employeeModel = _builder.GetEmployeeModel(employeeObject);
            var employeeEntityModel = _entityBuilder.GetEmployeeEntityModel(employeeModel);
            _repository.SaveEmployeeInfo(employeeEntityModel);
            return View(employeeModel);
        }
    }
}