using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;

namespace QuickbooksApi.Repository
{
    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {
        public void SaveEmployeeInfo(EmployeeInfo model)
        {
            Logger.WriteDebug("Connecting to database server to insert/update employeeinfo.");
            try
            {
                using(var ctx = new Entities())
                {
                    var id = model.Id;
                    if (ctx.EmployeeInfoes.Any(e => e.Id == id))
                    {
                        ctx.Entry(model).State = EntityState.Modified;
                    }
                    else
                    {
                        ctx.Entry(model).State = EntityState.Added;
                    }
                    ctx.SaveChanges();
                }
                Logger.WriteDebug("Saved employee info succesfully.");
            }
            catch(Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database to save to employee info.");
                throw e;
            }
        }

        public void DeleteEmployee(string id)
        {
            Logger.WriteDebug("Connecting to database server to delete employeeinfo.");
            try
            {
                using (var ctx = new Entities())
                {
                    var employee = ctx.EmployeeInfoes.FirstOrDefault(x => x.Id == id);
                    if (employee != null)
                    {
                        ctx.Entry(employee).State = EntityState.Deleted;
                        ctx.SaveChanges();
                    }
                }
                Logger.WriteDebug("Deleted employee info successfully");
            }
            catch(Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database to delete employee info");
                throw e;
            }
        }
    }
}