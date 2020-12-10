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
    public class AccountController : Controller
    {
        private JsonToModelBuilder _builder = new JsonToModelBuilder();
        private AccountRepository _repository = new AccountRepository();
        private ApiDataProvider _provider = new ApiDataProvider();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAccount(AccountInfo model)
        {
            AccountInfo acct = new AccountInfo()
            {
                Name = model.Name,
                AccountType = model.AccountType,
                Classification = model.Classification,
                CurrentBalance = model.CurrentBalance
            };
            var requestBody = new StringContent(JsonConvert.SerializeObject(acct), Encoding.UTF8, "application/json");

            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/account?minorversion=55", qboBaseUrl, realmId);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            var acctInfo = await _provider.Post(uri, requestBody, token);
            AccountInfo accountInfo = _builder.GetAccountModel(acctInfo);
            _repository.SaveAccountInfo(accountInfo);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> AccountDetails()
        {
            var accountId = WebUtility.UrlEncode("93");
            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/account/{2}?minorversion=55", qboBaseUrl, realmId, accountId);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            var accountObject = await _provider.Get(uri, token);
            var accountInfo = _builder.GetAccountModel(accountObject);
            _repository.SaveAccountInfo(accountInfo);
            return View(accountInfo);
        }


        //To do
        public async Task<ActionResult> UpdateAccount()
        {
            var account = new 
            {
                FullyQualifiedName = "Shuvo New Credit",
                domain = "QBO",
                SubAccount = false,
                Description = "Description added during update.",
                Classification = "Asset",
                AccountSubType = "AccountsReceivable",
                CurrentBalanceWithSubAccounts = 1091.23,
                sparse = false,
                MetaData = new
                {
                    CreateTime = "2014-09-12T10:12:02-07:00",
                    LastUpdatedTime = "2015-06-30T15:09:07-07:00"
                },
                AccountType = "Accounts Receivable",
                CurrentBalance = 1091.23,
                Active = true,
                SyncToken = "0",
                Id = "93",
                Name = "Shuvo New Credit"
            };
            var requestBody = new StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json");

            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/account?minorversion=55", qboBaseUrl, realmId);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            var accountInfo = await _provider.Post(uri, requestBody, token); 
            return RedirectToAction("Index");
        }
    }
}