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
            IJsonToModelBuilder builder, IApiModelToEntityModelBuilder entityBuilder): base(provider, builder, entityBuilder)
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
        public async Task<ActionResult> CreateItem(ItemModel model)
        {
            Logger.WriteDebug("Creating new item.");
            model.IncomeAccountRef = new IncomeAccount()
            {
                name = "Sales of Product Income",
                value = "79"
            };
            model.ExpenseAccountRef = new ExpenseAccount()
            {
                name = "Cost of Goods Sold",
                value = "80"
            };

            var requestBody = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var itemObj = await HandlePostRequest(requestBody, EntityType.Item.ToString().ToLower());
            ItemModel itemModel = _builder.GetItemModel(itemObj);
            var itemEntityModel = _entityBuilder.GetItemEntityModel(itemModel);
            _repository.SaveItemInfo(itemEntityModel);

            return RedirectToAction("Index");
        }

        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory(CategoryModel model)
        {
            var category = new CategoryModel()
            {
                Name = model.Name,
                Type = "Category",
                Active = model.Active,
                SyncToken = model.SyncToken
            };
            var requestBody = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
            var categoryObj = await HandlePostRequest(requestBody, EntityType.Item.ToString().ToLower(), "4");
            ItemModel categoryModel = _builder.GetItemModel(categoryObj);
            ItemInfo categoryEntityModel = _entityBuilder.GetCategoryEntityModel(categoryModel);
            _repository.SaveCategoryInfo(categoryEntityModel);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> BundleDetails()
        {
            string bundleId = "28";
            var itemObj = await HandleGetRequest(bundleId, EntityType.Item.ToString().ToLower());
            ItemModel itemModel = _builder.GetItemModel(itemObj);
            var itemEntityModel = _entityBuilder.GetItemEntityModel(itemModel);
            _repository.SaveItemInfo(itemEntityModel);
            return View(itemModel);
        }

        public async Task<ActionResult> CategoryDetails()
        {
            string categoryId = "27";
            var categoryObj = await HandleGetRequest(categoryId, EntityType.Item.ToString().ToLower());
            var categoryModel = _builder.GetItemModel(categoryObj);
            var categoryEntityModel = _entityBuilder.GetCategoryEntityModel(categoryModel);
            _repository.SaveCategoryInfo(categoryEntityModel);
            return View(categoryModel);
        }

        public ActionResult UpdateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCategory(string name, string syncToken)
        {
            var category = new CategoryModel()
            {
                Id = "27",
                Name = name,
                SyncToken = syncToken,
                Type = "Category",
                Active = true
            };
            var requestBody = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
            var categoryObj = await HandlePostRequest(requestBody, EntityType.Item.ToString().ToLower(), "4");
            ItemModel categoryModel = _builder.GetItemModel(categoryObj);
            var categoryEntityModel = _entityBuilder.GetCategoryEntityModel(categoryModel);
            _repository.SaveCategoryInfo(categoryEntityModel);
            return RedirectToAction("Index");
        }
    }
}