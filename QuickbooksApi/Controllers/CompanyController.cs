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
    public class CompanyController : BaseController
    {
        private ICompanyRepository _repository;

        public CompanyController(
            ICompanyRepository repository, IApiDataProvider provider,
            IJsonToModelBuilder builder, IApiModelToEntityModelBuilder entityBuilder) : base(provider, builder, entityBuilder)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async  Task<ActionResult> CompanyDetails()
        {
            Logger.WriteDebug("Showing company details");
            var realmId = Session["realmId"].ToString();
            var companyObject = await HandleGetRequest(realmId, "companyinfo");
            var companyModel = _builder.GetCompanyModel(companyObject);
            CompanyInfo companyEntityModel = _entityBuilder.GetCompanyEntityModel(companyModel);
            _repository.SaveCompanyDetails(companyEntityModel);
            return View(companyModel);
        }

        public ActionResult UpdateCompany()
        {
            CompanyInfo company = _repository.GetCompanyInfo("1");
            return View(company);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCompany(CompanyInfo model)
        {
            Logger.WriteDebug("Updating company details.");
            var syncToken = _repository.GetSyncToken("1");
            var company = new CompanyModel
            {
                SyncToken = syncToken,
                CompanyName = model.CompanyName,
                Id = "1"
            };
            var requestBody = new StringContent(JsonConvert.SerializeObject(company), Encoding.UTF8, "application/json");
            var companyObj = await HandlePostRequest(requestBody, "companyinfo");
            var companyModel = _builder.GetCompanyModel(companyObj);
            CompanyInfo companyEntityModel = _entityBuilder.GetCompanyEntityModel(companyModel);
            _repository.SaveCompanyDetails(companyEntityModel);

            return RedirectToAction("Index");
        }
    }
}