using Newtonsoft.Json;
using Quickbooks.Report.Interfaces;
using Quickbooks.Report.Models;
using QuickbooksAPI.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quickbooks.Report.ReportProvider
{
    public class CustomerBalanceReportProvider : BaseProvider, ICustomerBalanceReportProvider
    {
        public CustomerBalanceReportProvider(IApiDataProvider provider): base(provider)
        {

        }

        public async Task<List<CustomerBalance> > GetCustomerBalance(string baseUri, string entity, string realmId, string accessToken, string id)
        {
            var json = await HandleRequest(baseUri, entity, realmId, id, accessToken);
            var model = JsonConvert.DeserializeObject<QuickbooksReport>(json);

            var customerBalance = new List<CustomerBalance>();
            customerBalance.Add(new CustomerBalance() {
                CustomerName = model.Rows.Row[0].ColData[0].Value,
                Balance = model.Rows.Row[0].ColData[1].Value
            });
            return customerBalance;
        }
    }
}
