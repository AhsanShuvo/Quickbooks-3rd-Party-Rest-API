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
    }
}