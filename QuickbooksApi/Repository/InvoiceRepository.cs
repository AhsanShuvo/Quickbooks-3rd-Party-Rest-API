using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;

namespace QuickbooksApi.Repository
{
    public class InvoiceRepository : BaseRepository, IInvoiceRepository
    {
        public void SaveInvoiceInfo(InvoiceInfo model)
        {
            try
            {
                using (var ctx = new Entities())
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
            catch(Exception e)
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
                using (var ctx = new Entities())
                {
                    var invoice = ctx.InvoiceInfoes.Find(id);
                    ctx.InvoiceInfoes.Attach(invoice);
                    ctx.InvoiceInfoes.Remove(invoice);
                    ctx.SaveChanges();
                }
                Logger.WriteDebug("Deleted invoice successfully");
            }
            catch(Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database to delete invoice info.");
                throw e;
            }
        }
    }
}