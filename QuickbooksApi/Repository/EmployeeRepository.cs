using QuickbooksApi.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace QuickbooksApi.Repository
{
    public class EmployeeRepository
    {
        public void SaveEmployeeInfo(EmployeeInfo model)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["QuickbooksDB"].ConnectionString;
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    var qry = @"IF EXISTS(SELECT * FROM EmployeeInfo WHERE Id = @Id)
                                    BEGIN
                                        UPDATE EmployeeInfo
                                        SET GivenName = @GivenName, DisplayName = @DisplayName, Active = @Active, SyncToken = @SyncToken
                                        WHERE Id = @Id
                                    END
                                ELSE
                                    BEGIN
                                        INSERT INTO EmployeeInfo(Id, GivenName, DisplayName, Active, SyncToken) 
                                        VALUES(@Id, @GIvenName, @DisplayName, @Active, @SyncToken)
                                    END";

                    SqlCommand cmd = new SqlCommand(qry, con);
                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@GivenName", model.GivenName);
                    cmd.Parameters.AddWithValue("@DisplayName", model.DisplayName);
                    cmd.Parameters.AddWithValue("@Active", model.Active);
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
                catch (Exception e)
                {
                    throw (e);
                }
            }
        }
    }
}