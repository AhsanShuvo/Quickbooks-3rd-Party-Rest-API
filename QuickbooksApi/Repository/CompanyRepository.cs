using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;

namespace QuickbooksApi.Repository
{
    public class CompanyRepository : BaseRepository, ICompanyRepository
    {
        public void SaveCompanyDetails(CompanyInfo model)
        {
            Logger.WriteDebug("Connecting to database server to update companyinfo.");
            try
            {
                using (var ctx = new Entities())
                {
                    var id = model.Id;
                    if (ctx.CompanyInfoes.Any(e => e.Id == id))
                    {
                        ctx.Entry(model).State = EntityState.Modified;
                    }
                    else
                    {
                        ctx.Entry(model).State = EntityState.Added;
                    }
                    ctx.SaveChanges();
                    Logger.WriteDebug("Saved company info successfully.");
                }
            }
            catch(Exception e)
            {
                Logger.WriteError(e, "Unable to connect to database.");
                throw e;
            }
        }

        public string GetSyncToken(string id)
        {
            string syncToken = string.Empty;
            Logger.WriteDebug("Connecting to the database server to get synctoken.");
            try
            {
                using(var ctx = new Entities())
                {
                    syncToken = ctx.CompanyInfoes.FirstOrDefault(x => x.Id == id).SyncToken;
                }
                Logger.WriteDebug("Fetched synctoken successfully.");
            }
            catch(Exception e)
            {
                Logger.WriteError(e, "Failed to connect to database to get synctoken.");
                throw e;
            }
            return syncToken;
        }

        public CompanyInfo GetCompanyInfo(string id)
        {
            CompanyInfo company = new CompanyInfo();
            Logger.WriteDebug("Connecting to the database server to fetch company info.");
            try
            {
                using(var ctx = new Entities()){
                    company = ctx.CompanyInfoes.FirstOrDefault(x => x.Id == id);
                }
                Logger.WriteDebug("Fetched company info successfully.");
            }
            catch(Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database to fetch company info.");
                throw e;
            }
            return company;
        }
    }
}