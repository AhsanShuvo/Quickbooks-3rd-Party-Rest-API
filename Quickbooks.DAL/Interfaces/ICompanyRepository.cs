namespace QuickbooksDAL.Interfaces
{
    public interface ICompanyRepository
    {
        void SaveCompanyDetails(CompanyInfo model);
        string GetSyncToken(string id);
        CompanyInfo GetCompanyInfo(string id);
    }
}
