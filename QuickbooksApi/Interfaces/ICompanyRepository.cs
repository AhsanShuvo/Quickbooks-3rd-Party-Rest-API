using QuickbooksApi.Models;

namespace QuickbooksApi.Interfaces
{
    public interface ICompanyRepository
    {
        void SaveCompanyDetails(CompanyInfo model);
        string GetSyncToken(string id);
        CompanyInfo GetCompanyInfo(string id);
    }
}
