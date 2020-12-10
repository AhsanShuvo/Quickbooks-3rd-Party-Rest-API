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
    public class EmployeeController : Controller
    {
        private JsonToModelBuilder _builder = new JsonToModelBuilder();
        private EmployeeRepository _repository = new EmployeeRepository();
        private ApiDataProvider _provider = new ApiDataProvider();

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
            EmployeeInfo employee = new EmployeeInfo()
            {
                GivenName = model.GivenName,
                FamilyName = model.DisplayName,
                Active = model.Active
            };
            var requestBody = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");

            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/employee?minorversion=55", qboBaseUrl, realmId);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            var employeeObject = await _provider.Post(uri, requestBody, token);
            EmployeeInfo customerInfo = _builder.GetEmployeeModel(employeeObject);
            _repository.SaveEmployeeInfo(customerInfo);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> EmployeeDetails()
        {
            var employeeId = "77";
            var id = WebUtility.UrlEncode(employeeId);
            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/employee/{2}?minorversion=55", qboBaseUrl, realmId, id);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            var employeeObject = await _provider.Get(uri, token);
            EmployeeInfo employee = _builder.GetEmployeeModel(employeeObject);
            _repository.SaveEmployeeInfo(employee);
            return View(employee);
        }
    }
}