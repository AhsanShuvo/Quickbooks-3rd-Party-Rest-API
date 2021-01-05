using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;

namespace QuickbooksApi.Repository
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        public void SaveCustomerDetails(CustomerInfo model)
        {
            Logger.WriteDebug("Connecting to database server to insert/update customerinfo.");
            try
            {
                using(var ctx = new Entities())
                {
                    var id = model.Id;
                    if (ctx.CustomerInfoes.Any(e => e.Id == id))
                    {
                        ctx.Entry(model).State = EntityState.Modified;
                    }
                    else
                    {
                        ctx.Entry(model).State = EntityState.Added;
                    }
                    ctx.SaveChanges();
                    Logger.WriteDebug("Saved CustomerDetails successully.");
                }
            }
            catch(Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database.");
            }
        }

        public void DeleteCustomer(string id)
        {
            Logger.WriteDebug("Connecting to the database to delete customer info.");
            try
            {
                using(var ctx = new Entities())
                {
                    var customer = ctx.CustomerInfoes.Find(id);
                    ctx.CustomerInfoes.Attach(customer);
                    ctx.CustomerInfoes.Remove(customer);
                    ctx.SaveChanges();
                }
                Logger.WriteDebug("Deleted customer info successfully.");
            }
            catch(Exception e)
            {
                Logger.WriteError(e, "Failed to connect to database to delete customer info");
            }
        }
    }
}