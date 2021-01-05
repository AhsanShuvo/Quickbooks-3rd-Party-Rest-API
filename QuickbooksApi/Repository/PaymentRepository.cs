﻿using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;

namespace QuickbooksApi.Repository
{
    public class PaymentRepository : BaseRepository, IPaymentRepository
    {
        // Caution: Database table doesn't match Payment model
        public void SavePaymentInfo(PaymentInfo model)
        {
            Logger.WriteDebug("Connecting to database server to insert/update paymentinfo.");
            try
            {
                using(var ctx = new Entities())
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
                Logger.WriteDebug("Saved payment info successfully.");
            }
            catch(Exception e)
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
                using(var ctx = new Entities())
                {
                    payment = ctx.PaymentInfoes.Where(p => p.Id == id)
                             .FirstOrDefault<PaymentInfo>();
                }
                Logger.WriteDebug("Fetched payment info successfully.");
            }
            catch(Exception e)
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
                using(var ctx = new Entities())
                {
                    var payment = ctx.PaymentInfoes.Find(id);
                    ctx.PaymentInfoes.Attach(payment);
                    ctx.PaymentInfoes.Remove(payment);
                    ctx.SaveChanges();
                }
            }
            catch(Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database to delete payment");
                throw e;
            }
        }
    }
}