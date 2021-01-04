using QuickbooksApi.Models;

namespace QuickbooksApi.Interfaces
{
    public interface ICustomerRepository
    {
        void SaveCustomerDetails(CustomerInfo model);
        void DeleteCustomer(string id);
    }
}
