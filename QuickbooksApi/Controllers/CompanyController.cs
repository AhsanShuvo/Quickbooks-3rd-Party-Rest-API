using Newtonsoft.Json;
using QuickbooksApi.ApiService;
using QuickbooksApi.ModelBuilder;
using QuickbooksApi.Repository;
using System.Configuration;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuickbooksApi.Controllers
{
    public class CompanyController : Controller
    {
        JsonToModelBuilder _builder = new JsonToModelBuilder();
        ApiDataProvider _provider = new ApiDataProvider();
        CompanyRepository _repository = new CompanyRepository();
        public ActionResult Index()
        {
            return View();
        }

        public async  Task<ActionResult> CompanyDetails()
        {
            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/companyinfo/{2}?minorversion=55", qboBaseUrl, realmId, realmId);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            var companyObject = await _provider.Get(uri, token);
            var companyInfo = _builder.GetCompanyModel(companyObject);
            _repository.SaveCompanyDetails(companyInfo);
            return View(companyInfo);
        }

        public async Task<ActionResult> UpdateCompany()
        {
            var company = new
            {
                SyncToken = "16",
                CompanyName = "Larry's Bakery",
                sparse = true,
                LegalName = "Larry Smith's Bakery",
                Id = "1"
            };
            var requestBody = new StringContent(JsonConvert.SerializeObject(company), Encoding.UTF8, "application/json");

            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/companyinfo?minorversion=55", qboBaseUrl, realmId);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            var companyObj = await _provider.Post(uri, requestBody, token);
            var companyInfo = _builder.GetCompanyModel(companyObj);
            _repository.SaveCompanyDetails(companyInfo);

            return RedirectToAction("Index");
        }
    }
}