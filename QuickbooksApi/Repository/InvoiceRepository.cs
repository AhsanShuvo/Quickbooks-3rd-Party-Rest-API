using QuickbooksApi.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace QuickbooksApi.Repository
{
    public class InvoiceRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["QuickbooksDB"].ConnectionString;

        public void SaveInvoiceInfo(InvoiceInfo model)
        {
            using(SqlConnection con = new SqlConnection())
            {
                var qry = @"IF EXISTS(SELECT * FROM InvoiceInfo WHERE Id = @Id)
                                BEGIN
                                    UPDATE InvoiceInfo
                                    SET TotalAmt = @TotalAmt, SyncToken = @SyncToken, TxnDate = @TxnDate
                                    WHERE Id = @Id    
                                END
                            ELSE
                                BEGIN
                                    INSERT INTO InvoiceInfo(Id, TotalAmt, SyncToken, TxnDate)
                                    VALUES(@Id, @TotalAmt, @SyncToken, @TxnDate)
                                END";
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@TotalAmt", model.TotalAmt);
                cmd.Parameters.AddWithValue("TxnDate", model.TxnDate);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void DeleteInvoiceInfo(string id)
        {
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                var qry = @"IF EXISTS(SELECT * FROM InvoiceInfo WHERE Id = @Id)
                                BEGIN
                                    DELETE FROM InvoiceInfo WHERE Id = @Id
                                END";

                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}