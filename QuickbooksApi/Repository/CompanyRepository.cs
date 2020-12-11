using QuickbooksApi.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace QuickbooksApi.Repository
{
    public class CompanyRepository
    {
        public void SaveCompanyDetails(CompanyInfo model)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["QuickbooksDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                var qry = @"IF EXISTS(SELECT * FROM CompanyInfo WHERE Id = @Id)
                                BEGIN
                                    UPDATE CompanyInfo
                                    SET CompanyName = @CompanyName, CompanyStartDate = @CompanyStartDate, SyncToken = @SyncToken
                                    WHERE Id = @Id
                                END
                            ELSE
                                BEGIN
                                    INSERT INTO CompanyInfo(Id, CompanyName, CompanyStartDate, SyncToken)
                                    VALUES(@Id, @CompanyName, @CompanyStartDate, @SyncToken)
                                END";

                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@id", model.Id);
                cmd.Parameters.AddWithValue("@Name", model.CompanyName);
                cmd.Parameters.AddWithValue("@AccountType", model.CompanyStartDate);
                cmd.Parameters.AddWithValue("@SyncToken", model.SyncToken);
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                }
                con.Close();
            }
        }
    }
}