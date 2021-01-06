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
                throw e;
            }
        }

        public void DeleteCustomer(string id)
        {
            Logger.WriteDebug("Connecting to the database to delete customer info.");
            try
            {
                using(var ctx = new Entities())
                {
                    var customer = ctx.CustomerInfoes.FirstOrDefault(x => x.Id == id);
                    if(customer != null)
                    {
                        ctx.Entry(customer).State = EntityState.Deleted;
                        ctx.SaveChanges();
                    }
                }
                Logger.WriteDebug("Deleted customer info successfully.");
            }
            catch(Exception e)
            {
                Logger.WriteError(e, "Failed to connect to database to delete customer info");
                throw e;
            }
        }

        public CustomerInfo GetCustomerInfo(string id)
        {
            CustomerInfo customer = new CustomerInfo();
            Logger.WriteDebug("Connecting to the database to fetch customer info.");
            try
            {
                using(var ctx = new Entities())
                {
                    customer = ctx.CustomerInfoes.FirstOrDefault(x => x.Id == id);
                }
                Logger.WriteDebug("Fetched customer info successfully.");
            }
            catch(Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database to fetch customer info.");
                throw e;
            }
            return customer;
        }

        public string GetSyncToken(string id)
        {
            string syncToken = string.Empty;
            Logger.WriteDebug("Connecting to the database server to fetch sync token.");
            try
            {
                using(var ctx = new Entities())
                {
                    syncToken = ctx.CustomerInfoes.FirstOrDefault(x => x.Id == id).SyncToken;
                }
            }
            catch(Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database server to fetch sync token.");
                throw e;
            }
            return syncToken;
        }
    }
}