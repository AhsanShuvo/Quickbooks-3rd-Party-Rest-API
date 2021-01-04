using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using QuickbooksApi.Models;
using System;
using System.Data.SqlClient;

namespace QuickbooksApi.Repository
{
    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {
        public void SaveEmployeeInfo(EmployeeInfo model)
        {
            Logger.WriteDebug("Connecting to database server to insert/update employeeinfo.");
            using (SqlConnection con = new SqlConnection(connectionString))
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
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Logger.WriteError(e, "Failed to connect to database to insert/update employeeinfo.");
                    throw e;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public void DeleteEmployee(string id)
        {
            Logger.WriteDebug("Connecting to database server to delete employeeinfo.");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                var qry = "DELETE FROM EmployeeInfo WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Id", id);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Logger.WriteError(e, "Failed to connect to database to delete employeeinfo.");
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