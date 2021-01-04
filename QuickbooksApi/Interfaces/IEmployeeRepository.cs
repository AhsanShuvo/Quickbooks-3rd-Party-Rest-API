using QuickbooksApi.Models;

namespace QuickbooksApi.Interfaces
{
    public interface IEmployeeRepository
    {
        void SaveEmployeeInfo(EmployeeInfo model);
        void DeleteEmployee(string id);
    }
}
