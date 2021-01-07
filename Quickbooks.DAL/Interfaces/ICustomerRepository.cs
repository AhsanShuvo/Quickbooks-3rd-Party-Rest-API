namespace QuickbooksDAL.Interfaces
{
    public interface ICustomerRepository
    {
        void SaveCustomerDetails(CustomerInfo model);
        void DeleteCustomer(string id);
        CustomerInfo GetCustomerInfo(string id);
        string GetSyncToken(string id);
    }
}
