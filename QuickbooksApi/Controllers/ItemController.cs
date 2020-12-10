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
    public class ItemController : Controller
    {
        private ApiDataProvider _provider = new ApiDataProvider();
        private JsonToModelBuilder _builder = new JsonToModelBuilder();
        private ItemRepository _repository = new ItemRepository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateItem()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateItem(ItemInfo model)
        {
            ItemInfo item = new ItemInfo()
            {
                Name = model.Name,
                Type = model.Type,
                Active = model.Active,
                UnitPrice = model.UnitPrice,
                QtyOnHand = model.QtyOnHand,
                PurchaseCost = model.PurchaseCost
            };

            item.IncomeAccountRef = new IncomeAccount()
            {
                name = "Sales of Product Income",
                value = "79"
            };
            item.ExpenseAccountRef = new ExpenseAccount()
            {
                name = "Cost of Goods Sold",
                value = "80"
            };

            var requestBody = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/item", qboBaseUrl, realmId);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            var itemObj = await _provider.Post(uri, requestBody, token);
            ItemInfo itemInfo = _builder.GetItemModel(itemObj);
            _repository.SaveItemInfo(itemInfo);

            return RedirectToAction("Index");
        }

        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory(CategoryInfo model)
        {
            var category = new CategoryInfo()
            {
                Name = model.Name,
                Type = "Category",
                Active = model.Active,
                SyncToken = model.SyncToken
            };

            var requestBody = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/item?minorversion=4", qboBaseUrl, realmId);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            var categoryObj = await _provider.Post(uri, requestBody, token);
            CategoryInfo categoryInfo = _builder.GetCategoryModel(categoryObj);
            _repository.SaveCategoryInfo(categoryInfo);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> BundleDetails()
        {
            string bundleId = "28";
            var id = WebUtility.UrlEncode(bundleId);
            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/item/{2}?minorversion=55", qboBaseUrl, realmId, id);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            var itemObj = await _provider.Get(uri, token);
            ItemInfo item = _builder.GetItemModel(itemObj);
            _repository.SaveItemInfo(item);

            return View(item);
        }

        public async Task<ActionResult> CategoryDetails()
        {
            string categoryId = "27";
            var id = WebUtility.UrlEncode(categoryId);
            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/item/{2}?minorversion=55", qboBaseUrl, realmId, id);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            var categoryObj = await _provider.Get(uri, token);
            var category = _builder.GetCategoryModel(categoryObj);
            _repository.SaveCategoryInfo(category);

            return View(category);
        }

        public ActionResult UpdateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCategory(string name, string syncToken)
        {
            var category = new CategoryInfo()
            {
                Id = "27",
                Name = name,
                SyncToken = syncToken,
                Type = "Category",
                Active = true
            };

            var requestBody = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/item?minorversion=4", qboBaseUrl, realmId);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            var categoryObj = await _provider.Post(uri, requestBody, token);
            CategoryInfo categoryInfo = _builder.GetCategoryModel(categoryObj);
            _repository.SaveCategoryInfo(categoryInfo);

            return RedirectToAction("Index");
        }
    }
}