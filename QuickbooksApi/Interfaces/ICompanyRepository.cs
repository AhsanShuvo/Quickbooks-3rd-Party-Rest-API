using QuickbooksApi.Models;

namespace QuickbooksApi.Interfaces
{
    public interface ICompanyRepository
    {
        void SaveCompanyDetails(CompanyInfo model);
    }
}
