using QuickbooksAPI.Interfaces;
using System.Threading.Tasks;

namespace Quickbooks.Report.ReportProvider
{
    public class BaseProvider
    {
        protected IApiDataProvider _provider;

        public BaseProvider(IApiDataProvider provider)
        {
            _provider = provider;
        }

        protected async Task<string> HandleRequest(string baseUrl, string entityType, string realmId, string id, string accessToken)
        {
            string uri = string.Format("{0}/v3/company/{1}/reports/{2}?customer={3}&minorversion=55", baseUrl, realmId, entityType, id);
            var result = await _provider.Get(uri, accessToken);
            return result;
        }
    }
}
