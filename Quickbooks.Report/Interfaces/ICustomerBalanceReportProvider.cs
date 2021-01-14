using Quickbooks.Report.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quickbooks.Report.Interfaces
{
    public interface ICustomerBalanceReportProvider
    {
        Task<List<CustomerBalance>> GetCustomerBalance(string baseUri, string entity, string realmId, string accessToken, string id);
    }
}
