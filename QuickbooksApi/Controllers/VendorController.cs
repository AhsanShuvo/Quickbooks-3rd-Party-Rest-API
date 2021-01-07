using QuickbooksAPI.Interfaces;
using QuickbooksCommon.Logger;
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
    public class VendorController : BaseController
    {
        private IVendorRepository _repository;

        public VendorController(
            IVendorRepository repository, IApiDataProvider provider,
            IJsonToModelBuilder builder, IApiModelToEntityModelBuilder entityBuilder): base(provider, builder, entityBuilder)
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
        public async Task<ActionResult> CreateVendor(VendorModel model)
        {
            Logger.WriteDebug("Creating a new vendor.");
            var requestBody = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var vendorObject = await HandlePostRequest(requestBody, EntityType.Vendor.ToString().ToLower());
            VendorModel vendorModel = _builder.GetVendorModel(vendorObject);
            var vendorEntityModel = _entityBuilder.GetVendorEntityModel(vendorModel);
            _repository.SaveVendorInfo(vendorEntityModel);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> VendorDetails()
        {
            var vendorObject = await HandleGetRequest("71", EntityType.Vendor.ToString().ToLower());
            var vendorModel = _builder.GetVendorModel(vendorObject);
            var vendorEntityModel = _entityBuilder.GetVendorEntityModel(vendorModel);
            _repository.SaveVendorInfo(vendorEntityModel);
            return View(vendorModel);
        }
    }
}