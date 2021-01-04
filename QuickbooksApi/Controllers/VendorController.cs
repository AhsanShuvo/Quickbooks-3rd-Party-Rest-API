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
    public class VendorController : BaseController
    {
        private IVendorRepository _repository;

        public VendorController(
            IVendorRepository repository, IApiDataProvider provider,
            IJsonToModelBuilder builder): base(provider, builder)
        {
            _repository = repository;
        }

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
            Logger.WriteDebug("Creating a new vendor.");
            VendorInfo vendor = new VendorInfo()
            {
                DisplayName = model.DisplayName,
                CompanyName = model.CompanyName,
                Active = model.Active,
                SyncToken = model.SyncToken,
                Balance = model.Balance
            };

            var requestBody = new StringContent(JsonConvert.SerializeObject(vendor), Encoding.UTF8, "application/json");
            var vendorObject = await HandlePostRequest(requestBody, EntityType.Vendor.ToString().ToLower());
            VendorInfo vendorInfo = _builder.GetVendorModel(vendorObject);
            _repository.SaveVendorInfo(vendorInfo);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> VendorDetails()
        {
            var vendorObject = await HandleGetRequest("71", EntityType.Vendor.ToString().ToLower());
            var vendorInfo = _builder.GetVendorModel(vendorObject);
            _repository.SaveVendorInfo(vendorInfo);
            return View(vendorInfo);
        }
    }
}