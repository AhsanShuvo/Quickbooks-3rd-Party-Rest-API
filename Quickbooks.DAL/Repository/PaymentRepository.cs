using QuickbooksCommon.Logger;
using QuickbooksDAL.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;

namespace QuickbooksDAL.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        public void SavePaymentInfo(PaymentInfo model)
        {
            Logger.WriteDebug("Connecting to database server to insert/update paymentinfo.");
            try
            {
                using (var ctx = new QuickbooksEntities())
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
                    var customer = ctx.CustomerInfoes.Find(model.CustomerRef);
                    customer.Balance += model.TotalAmt;
                    ctx.Entry(customer).State = EntityState.Modified;
                    ctx.SaveChanges();
                }
                Logger.WriteDebug("Saved payment info successfully.");
            }
            catch (Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database to save payment.");
                throw e;
            }
        }

        public PaymentInfo GetPaymentInfo(string id)
        {
            Logger.WriteDebug("Connecting to database server to get paymentinfo.");
            PaymentInfo payment = new PaymentInfo();
            try
            {
                using (var ctx = new QuickbooksEntities())
                {
                    payment = ctx.PaymentInfoes.Where(p => p.Id == id)
                             .FirstOrDefault<PaymentInfo>();
                }
                Logger.WriteDebug("Fetched payment info successfully.");
            }
            catch (Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database to get payment info.");
                throw e;
            }
            return payment;
        }

        public void DeletePayment(string id)
        {
            Logger.WriteDebug("Connecting to database server to delete paymentinfo.");
            try
            {
                using (var ctx = new QuickbooksEntities())
                {
                    var payment = ctx.PaymentInfoes.SingleOrDefault(p => p.Id == id);
                    if (payment != null)
                    {
                        ctx.Entry(payment).State = EntityState.Deleted;
                        ctx.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database to delete payment");
                throw e;
            }
        }
    }
}
