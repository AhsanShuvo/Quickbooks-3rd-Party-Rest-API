using QuickbooksApi.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace QuickbooksApi.Repository
{
    public class VendorRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["QuickbooksDB"].ConnectionString;

        public void SaveVendorInfo(VendorInfo model)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                var qry = @"IF EXISTS(SELECT * FROM VendorInfo WHERE Id = @Id)
                                BEGIN
                                    UPDATE VendorInfo
                                    SET DisplayName = @DisplayName, CompanyName = @CompanyName, Active = @Active, Balance = @Balance, SyncToken = @SyncToken
                                END
                            ELSE
                                BEGIN
                                    INSERT INTO VendorInfo(Id, DisplayName, CompanyName, Active, Balance, SyncToken)
                                    VALUES(@Id, @DisplayName, @CompanyName, @Active, @Balance, @SyncToken)
                                END";

                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@DisplayName", model.DisplayName);
                cmd.Parameters.AddWithValue("@CompanyName", model.CompanyName);
                cmd.Parameters.AddWithValue("@Active", model.Active);
                cmd.Parameters.AddWithValue("@Balance", model.Balance);
                cmd.Parameters.AddWithValue("@SyncToken", model.SyncToken);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}