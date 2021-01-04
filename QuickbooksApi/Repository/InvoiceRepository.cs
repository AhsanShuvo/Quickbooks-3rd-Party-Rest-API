using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using QuickbooksApi.Models;
using System;
using System.Data.SqlClient;

namespace QuickbooksApi.Repository
{
    public class InvoiceRepository : BaseRepository, IInvoiceRepository
    {
        public void SaveInvoiceInfo(InvoiceInfo model)
        {
            Logger.WriteDebug("Connecting to database server to insert/update invoiceinfo.");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                var qry = @"IF EXISTS(SELECT * FROM InvoiceInfo WHERE Id = @Id)
                                BEGIN
                                    UPDATE InvoiceInfo
                                    SET TotalAmt = @TotalAmt, SyncToken = @SyncToken, TxnDate = @TxnDate, CustomerId = @CustomerId
                                    WHERE Id = @Id    
                                END
                            ELSE
                                BEGIN
                                    INSERT INTO InvoiceInfo(Id, TotalAmt, SyncToken, TxnDate, CustomerId)
                                    VALUES(@Id, @TotalAmt, @SyncToken, @TxnDate, @CustomerId)
                                END";
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@TotalAmt", model.TotalAmt);
                cmd.Parameters.AddWithValue("@TxnDate", model.TxnDate);
                cmd.Parameters.AddWithValue("@CustomerId", model.CustomerRef.value);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Logger.WriteError(e, "Failed to connect to database to insert/update invoiceinfo.");
                    throw e;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public void DeleteInvoiceInfo(string id)
        {
            Logger.WriteDebug("Connecting to database server to delete invoiceinfo.");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                var qry = @"IF EXISTS(SELECT * FROM InvoiceInfo WHERE Id = @Id)
                                BEGIN
                                    DELETE FROM InvoiceInfo WHERE Id = @Id
                                END";

                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Id", id);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Logger.WriteError(e, "Failed to connect to database to delete invoiceinfo.");
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