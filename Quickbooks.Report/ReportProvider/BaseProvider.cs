using QuickbooksDAL;
using QuickbooksDAL.Interfaces;
using System.Collections.Generic;

namespace Quickbooks.Report.ReportProvider
{
    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class BaseProvider
    {
        private readonly IUserRepository _user;
        private static List<Employee> _lstEmployee = null;

        public BaseProvider(IUserRepository user)
        {
            _user = user;
        }

        protected UserInfo GetUserInfo()
        {
            var realmId = GetRealmId();
            var userInfo = _user.GetUserInfo(realmId);
            return userInfo;
        }

        public static List<Employee> GetAllEmployees()
        {
            if (_lstEmployee == null)
            {
                _lstEmployee = new List<Employee>();
                _lstEmployee.Add(new Employee()
                {
                    ID = 1,
                    Name = "Alok",
                    Age = 30
                });

                _lstEmployee.Add(new Employee()
                {
                    ID = 2,
                    Name = "Ashish",
                    Age = 30
                });

                _lstEmployee.Add(new Employee()
                {
                    ID = 3,
                    Name = "Jasdeep",
                    Age = 30
                });

                _lstEmployee.Add(new Employee()
                {
                    ID = 4,
                    Name = "Kamlesh",
                    Age = 31
                });
            }
            return _lstEmployee;
        }
        private string GetRealmId()
        {
            return "4620816365155280670";
        }
    }
}
