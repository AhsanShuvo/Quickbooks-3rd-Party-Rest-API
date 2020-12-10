using QuickbooksApi.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace QuickbooksApi.Repository
{
    public class CustomerRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["QuickbooksDB"].ConnectionString;
        public void SaveCustomerDetails(CustomerInfo model)
        {
            
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                var qry = @"IF EXISTS(SELECT * FROM CustomerInfo WHERE Id =@Id )
                              BEGIN        
                                    UPDATE CustomerInfo 
                                    SET DisplayName = @DisplayName, CompanyName = @CompanyName, Balance = @Balance, SyncToken = @SyncToken, Active = @Active
                                    WHERE Id = @Id
                               END
                         ELSE
                            BEGIN
                                INSERT INTO CustomerInfo(Id, DisplayName, CompanyName, Balance, SyncToken, Active)
                                VALUES(@Id, @DisplayName, @CompanyName, @Balance, @SyncToken, @Active)
                            END";
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@DisplayName", model.DisplayName);
                cmd.Parameters.AddWithValue("@CompanyName", model.CompanyName);
                cmd.Parameters.AddWithValue("@Balance", model.Balance);
                cmd.Parameters.AddWithValue("@SyncToken", model.SyncToken);
                cmd.Parameters.AddWithValue("@Active", model.Active);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}