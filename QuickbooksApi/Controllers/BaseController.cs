using QuickbooksApi.Interfaces;
using System.Configuration;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuickbooksApi.Controllers
{
    public class BaseController : Controller
    {
        protected IApiDataProvider _provider;
        protected IJsonToModelBuilder _builder;
        protected IApiModelToEntityModelBuilder _entityBuilder;

        public BaseController(
            IApiDataProvider provider, IJsonToModelBuilder builder, IApiModelToEntityModelBuilder entityBuilder)
        {
            _provider = provider;
            _builder = builder;
            _entityBuilder = entityBuilder;
        }

        protected string _qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];

        protected async Task<string> HandleGetRequest(string entityId, string entityType)
        {
            var entity = Url.Encode(entityType);
            var id = Url.Encode(entityId);
            string realmId = GetRealmId();
            string uri = string.Format("{0}/v3/company/{1}/{2}/{3}?minorversion=55", _qboBaseUrl, realmId, entity, id);
            var token = GetAccessToken();
            var result = await _provider.Get(uri, token);
            return result;
        }

        protected async Task<string> HandlePostRequest(StringContent requestBody, string entityType, string minorVersion="55")
        {
            string entity = Url.Encode(entityType);
            string version = Url.Encode(minorVersion);
            string realmId = GetRealmId();
            string uri = string.Format("{0}/v3/company/{1}/{2}?minorversion={3}", _qboBaseUrl, realmId, entity, version);
            var token = GetAccessToken();
            var result = await _provider.Post(uri, requestBody, token);
            return result;
        }

        protected string GetRealmId()
        {
            return Session["realmId"].ToString();
        }

        protected async Task HandleDeleteRequest(StringContent requestBody, string entityType)
        {
            string entity = Url.Encode(entityType);
            string realmId = GetRealmId();
            string uri = string.Format("{0}/v3/company/{1}/{2}?operation=delete&minorversion=55", _qboBaseUrl, realmId, entity);
            var token = GetAccessToken();
            await _provider.Post(uri, requestBody, token);
        }

        protected string GetAccessToken()
        {
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            return token;
        }
    }
}