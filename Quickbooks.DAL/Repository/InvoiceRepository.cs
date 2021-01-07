using QuickbooksCommon.Logger;
using QuickbooksDAL.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;

namespace QuickbooksDAL.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        public void SaveInvoiceInfo(InvoiceInfo model)
        {
            try
            {
                using (var ctx = new QuickbooksEntities())
                {
                    var id = model.Id;
                    if (ctx.InvoiceInfoes.Any(e => e.Id == id))
                    {
                        ctx.Entry(model).State = EntityState.Modified;
                    }
                    else
                    {
                        ctx.Entry(model).State = EntityState.Added;
                    }
                    ctx.SaveChanges();
                }
                Logger.WriteDebug("Saved invoice successfully.");
            }
            catch (Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database to save invoice info.");
                throw e;
            }
        }

        public void DeleteInvoiceInfo(string id)
        {
            Logger.WriteDebug("Connecting to database server to delete invoiceinfo.");
            try
            {
                using (var ctx = new QuickbooksEntities())
                {
                    var invoice = ctx.InvoiceInfoes.FirstOrDefault(x => x.Id == id);
                    if (invoice != null)
                    {
                        ctx.Entry(invoice).State = EntityState.Deleted;
                        ctx.SaveChanges();
                    }
                }
                Logger.WriteDebug("Deleted invoice successfully.");
            }
            catch (Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database to delete invoice info.");
                throw e;
            }
        }
    }
}
