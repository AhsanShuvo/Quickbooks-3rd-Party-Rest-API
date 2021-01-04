using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using QuickbooksApi.Models;
using System;
using System.Data.SqlClient;

namespace QuickbooksApi.Repository
{
    public class CompanyRepository : BaseRepository, ICompanyRepository
    {
        public void SaveCompanyDetails(CompanyInfo model)
        {
            Logger.WriteDebug("Connecting to database server to update companyinfo.");
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
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Logger.WriteError(e, "Failed to connect to database server to update companyinfo.");
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