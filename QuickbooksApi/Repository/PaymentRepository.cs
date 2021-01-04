using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using QuickbooksApi.Models;
using System;
using System.Data.SqlClient;

namespace QuickbooksApi.Repository
{
    public class PaymentRepository : BaseRepository, IPaymentRepository
    {
        // Caution: Database table doesn't match Payment model
        public void SavePaymentInfo(PaymentInfo model)
        {
            Logger.WriteDebug("Connecting to database server to insert/update paymentinfo.");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                var qry = @"IF EXISTS(SELECT * FROM PaymentInfo WHERE Id = @Id)
                            BEGIN
                                UPDATE PaymentInfo
                                SET TotalAmt = @TotalAmt, SyncToken = @SyncToken, CustomerRef = @CustomerRef
                                WHERE Id = @Id
                            END
                          ELSE 
                            BEGIN
                                INSERT INTO PaymentInfo(Id, TotalAmt, SyncToken, CustomerRef)
                                VALUES(@Id, @TotalAmt, @SyncToken, @CustomerRef)
                            END";

                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@TotalAmt", model.TotalAmt);
                cmd.Parameters.AddWithValue("@SyncToken", model.SyncToken);
                cmd.Parameters.AddWithValue("@CustomerRef", model.CustomerRef.value);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Logger.WriteError(e, "Failed to connect to database to insert/update paymentinfo.");
                    throw e;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public PaymentInfo GetPaymentInfo(string id)
        {
            Logger.WriteDebug("Connecting to database server to get paymentinfo.");
            PaymentInfo payment = new PaymentInfo();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                var qry = @"SELECT * FROM PaymentInfo WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Id", id);
                try
                {
                    con.Open();
                    SqlDataReader rd = cmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            payment.Id = rd.GetString(0);
                            payment.TotalAmt = rd.GetDouble(1);
                            payment.SyncToken = rd.GetString(2);
                            payment.CustomerRef.value = rd.GetString(3);
                        }
                    }
                }
                catch(Exception e)
                {
                    Logger.WriteError(e, "Failed to connect to database to get paymentinfo.");
                    throw e;
                }
                finally
                {
                    con.Close();
                }
            }
            return payment;
        }

        public void DeletePayment(string id)
        {
            Logger.WriteDebug("Connecting to database server to delete paymentinfo.");
            var qry = @"IF EXISTS(SELECT * FROM PaymentInfo WHERE Id=@Id)
                            BEGIN
                                DELETE FROM PaymentInfo Where Id = @Id
                            END";

            using(SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Id", id);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Logger.WriteError(e, "Failed to connect to database to delete paymentinfo.");
                    throw e;
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}