using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using QuickbooksApi.Models;
using System;
using System.Data.SqlClient;

namespace QuickbooksApi.Repository
{
    public class VendorRepository : BaseRepository, IVendorRepository
    {
        public void SaveVendorInfo(VendorInfo model)
        {
            Logger.WriteDebug("Connecting to database server to insert/update vendorinfo.");
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
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Logger.WriteError(e, "Failed to connect to database to insert/update vendorinfo.");
                    throw e;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public void DeleteVendor(string id)
        {
            Logger.WriteDebug("Connecting to database server to delete vendorinfo.");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                var qry = @"IF EXISTS(SELECT * FROM VendorInfo WHERE Id=@Id)
                                BEGIN
                                    DELETE FROM VendorInfo WHERE Id=@Id
                                END";

                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Id", id);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Logger.WriteError(e, "Failed to connect to database to delete vendorinfo.");
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