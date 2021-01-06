using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;

namespace QuickbooksApi.Repository
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        public void SaveAccountInfo(AccountInfo model)
        {
            Logger.WriteDebug("Connecting to the database server to insert/update account.");
            try
            {
                using (var ctx = new Entities())
                {
                    var id = model.Id;
                    if (ctx.AccountInfoes.Any(e => e.Id == id))
                    {
                        ctx.Entry(model).State = EntityState.Modified;
                    }
                    else
                    {
                        ctx.Entry(model).State = EntityState.Added;
                    }
                    ctx.SaveChanges();
                }
                Logger.WriteDebug("Saved account info successfully.");
            }
            catch(Exception e)
            {
                Logger.WriteError(e, "Failed to connect to database to save account info.");
                throw e;
            }
        }

        public void  DeleteAccountInfo(string id)
        {
            Logger.WriteDebug("Connecting to database server to delete account.");
            try
            {
                using (var ctx = new Entities())
                {
                    var account = ctx.AccountInfoes.FirstOrDefault(x => x.Id == id);
                    if(account != null)
                    {
                        ctx.Entry(account).State = EntityState.Deleted;
                        ctx.SaveChanges();
                    }
                }
                Logger.WriteDebug("Deleted account successfully");
            }
            catch(Exception e)
            {
                Logger.WriteError(e, "Failed to connect to database to delete account info.");
                throw e;
            }
        }
    }
}