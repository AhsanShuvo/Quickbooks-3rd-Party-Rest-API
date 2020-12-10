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
    public class VendorController : Controller
    {
        ApiDataProvider _provider = new ApiDataProvider();
        JsonToModelBuilder _builder = new JsonToModelBuilder();
        VendorRepository _repository = new VendorRepository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateVendor()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateVendor(VendorInfo model)
        {
            VendorInfo vendor = new VendorInfo()
            {
                DisplayName = model.DisplayName,
                CompanyName = model.CompanyName,
                Active = model.Active,
                SyncToken = model.SyncToken,
                Balance = model.Balance
            };

            var requestBody = new StringContent(JsonConvert.SerializeObject(vendor), Encoding.UTF8, "application/json");

            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/vendor?minorversion=55", qboBaseUrl, realmId);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            var vendorObject = await _provider.Post(uri, requestBody, token);
            VendorInfo vendorInfo = _builder.GetVendorModel(vendorObject);
            _repository.SaveVendorInfo(vendorInfo);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> VendorDetails()
        {
            var id = WebUtility.UrlEncode("71");
            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/vendor/{2}?minorversion=55", qboBaseUrl, realmId, id);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            var vendorObject = await _provider.Get(uri, token);
            var vendorInfo = _builder.GetVendorModel(vendorObject);
            _repository.SaveVendorInfo(vendorInfo);
            return View(vendorInfo);
        }
    }
}