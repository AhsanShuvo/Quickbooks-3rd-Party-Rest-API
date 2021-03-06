using QuickbooksCommon.Logger;
using QuickbooksDAL.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;

namespace QuickbooksDAL.Repository
{
    public class UserRepository : IUserRepository
    {
        public void SaveUserInfo(UserInfo model)
        {
            Logger.WriteDebug("Connecting to database server to insert/update userinfo.");
            try
            {
                using (var ctx = new QuickbooksEntities())
                {
                    var id = model.RealmId;
                    if (ctx.UserInfoes.Any(e => e.RealmId == id))
                    {
                        ctx.Entry(model).State = EntityState.Modified;
                    }
                    else
                    {
                        ctx.Entry(model).State = EntityState.Added;
                    }
                    ctx.SaveChanges();
                }
                Logger.WriteDebug("Saved user info successfully");
            }
            catch (Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database to save user info.");
                throw e;
            }
        }

        public UserInfo GetUserInfo(string realmId)
        {
            Logger.WriteDebug("Connecting to database server to get userinfo.");
            UserInfo user = new UserInfo();
            try
            {
                using (var ctx = new QuickbooksEntities())
                {
                    user = ctx.UserInfoes.Where(u => u.RealmId == realmId)
                            .FirstOrDefault<UserInfo>();
                }
                Logger.WriteDebug("Fetched user info successfully.");
            }
            catch (Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database to fectch user info.");
                throw e;
            }
            return user;
        }
    }
}
