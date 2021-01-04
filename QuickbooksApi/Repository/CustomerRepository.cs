using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using QuickbooksApi.Models;
using System;
using System.Data.SqlClient;

namespace QuickbooksApi.Repository
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        public void SaveCustomerDetails(CustomerInfo model)
        {
            Logger.WriteDebug("Connecting to database server to insert/update customerinfo.");
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
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Logger.WriteError(e, "Failed to connect to database to insert/update customerinfo.");
                    throw e;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public void DeleteCustomer(string id)
        {
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                Logger.WriteDebug("Connecting to database server to delete customerinfo.");
                var qry = "DELETE FROM CustomerInfo WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Id", id);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Logger.WriteError(e, "Failed to connect to database server to delete customerinfo");
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