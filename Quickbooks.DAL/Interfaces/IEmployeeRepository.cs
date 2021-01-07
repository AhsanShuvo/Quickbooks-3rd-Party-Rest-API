namespace QuickbooksDAL.Interfaces
{
    public interface IEmployeeRepository
    {
        void SaveEmployeeInfo(EmployeeInfo model);
        void DeleteEmployee(string id);
    }
}
