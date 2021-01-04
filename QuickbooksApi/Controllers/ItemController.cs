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
    public class ItemController : BaseController
    {
        private IItemRepository _repository;

        public ItemController(
            IItemRepository repository, IApiDataProvider provider,
            IJsonToModelBuilder builder): base(provider, builder)
        {
            _repository = repository;
        }

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
            Logger.WriteDebug("Creating new item.");
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
            var itemObj = await HandlePostRequest(requestBody, EntityType.Item.ToString().ToLower());
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
            var categoryObj = await HandlePostRequest(requestBody, EntityType.Item.ToString().ToLower(), "4");
            ItemInfo categoryInfo = _builder.GetItemModel(categoryObj);
            _repository.SaveCategoryInfo(categoryInfo);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> BundleDetails()
        {
            string bundleId = "28";
            var itemObj = await HandleGetRequest(bundleId, EntityType.Item.ToString().ToLower());
            ItemInfo item = _builder.GetItemModel(itemObj);
            _repository.SaveItemInfo(item);
            return View(item);
        }

        public async Task<ActionResult> CategoryDetails()
        {
            string categoryId = "27";
            var categoryObj = await HandleGetRequest(categoryId, EntityType.Item.ToString().ToLower());
            var category = _builder.GetItemModel(categoryObj);
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
            var categoryObj = await HandlePostRequest(requestBody, EntityType.Item.ToString().ToLower(), "4");
            ItemInfo categoryInfo = _builder.GetItemModel(categoryObj);
            _repository.SaveCategoryInfo(categoryInfo);
            return RedirectToAction("Index");
        }
    }
}