using QuickbooksApi.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace QuickbooksApi.Repository
{
    public class PaymentRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["QuickbooksDB"].ConnectionString;

        // Caution: Database table doesn't match Payment model
        public void SavePaymentInfo(PaymentInfo model)
        {
            using(SqlConnection con = new SqlConnection(connectionString))
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

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public PaymentInfo GetPaymentInfo(string id)
        {
            PaymentInfo payment = new PaymentInfo();
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                var qry = @"SELECT * FROM PaymentInfo WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Id", id);

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
            return payment;
        }
    }
}