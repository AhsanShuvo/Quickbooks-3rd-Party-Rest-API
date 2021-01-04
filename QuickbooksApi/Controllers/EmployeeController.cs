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
            IJsonToModelBuilder builder): base(provider, builder)
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
        public async Task<ActionResult> CreateEmployee(EmployeeInfo model)
        {
            Logger.WriteDebug("Creating new employee");
            EmployeeInfo employee = new EmployeeInfo()
            {
                GivenName = model.GivenName,
                FamilyName = model.DisplayName,
                Active = model.Active
            };
            var requestBody = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");
            var employeeObject = await HandlePostRequest(requestBody, EntityType.Employee.ToString().ToLower());
            EmployeeInfo customerInfo = _builder.GetEmployeeModel(employeeObject);
            _repository.SaveEmployeeInfo(customerInfo);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> EmployeeDetails()
        {
            var employeeId = "77";
            var employeeObject = await HandleGetRequest(employeeId, EntityType.Employee.ToString().ToLower());
            EmployeeInfo employee = _builder.GetEmployeeModel(employeeObject);
            _repository.SaveEmployeeInfo(employee);
            return View(employee);
        }
    }
}