using QuickbooksCommon.Logger;
using QuickbooksDAL.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;

namespace QuickbooksDAL.Repository
{
    public class VendorRepository : IVendorRepository
    {
        public void SaveVendorInfo(VendorInfo model)
        {
            Logger.WriteDebug("Connecting to database server to insert/update vendorinfo.");
            try
            {
                using (var ctx = new QuickbooksEntities())
                {
                    var id = model.Id;
                    if (ctx.VendorInfoes.Any(e => e.Id == id))
                    {
                        ctx.Entry(model).State = EntityState.Modified;
                    }
                    else
                    {
                        ctx.Entry(model).State = EntityState.Added;
                    }
                    ctx.SaveChanges();
                }
                Logger.WriteDebug("Saved vendor info successfully.");
            }
            catch (Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database to save vendor info.");
                throw e;
            }
        }

        public void DeleteVendor(string id)
        {
            Logger.WriteDebug("Connecting to database server to delete vendorinfo.");
            try
            {
                using (var ctx = new QuickbooksEntities())
                {
                    var vendor = ctx.VendorInfoes.FirstOrDefault(v => v.Id == id);
                    if (vendor != null)
                    {
                        ctx.Entry(vendor).State = EntityState.Deleted;
                        ctx.SaveChanges();
                    }
                }
                Logger.WriteDebug("Deleted vendor info successfully.");
            }
            catch (Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database server to delete vendor.");
            }
        }
    }
}
