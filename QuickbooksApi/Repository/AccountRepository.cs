using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using QuickbooksApi.Models;
using System;
using System.Data.SqlClient;

namespace QuickbooksApi.Repository
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        public void SaveAccountInfo(AccountInfo model)
        {
            Logger.WriteDebug("Connecting to the database server to insert/update account.");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                var qry = @"IF EXISTS(SELECT * FROM AccountInfo WHERE Id = @Id)
                                BEGIN
                                    UPDATE AccountInfo
                                    SET Name = @Name, AccountType = @AccountType, Classification = @Classification, CurrentBalance = @CurrentBalance, SyncToken = @SyncToken
                                    WHERE Id = @Id
                                END
                            ELSE
                                BEGIN
                                    INSERT INTO AccountInfo(Id, Name, AccountType, Classification, CurrentBalance, SyncToken)
                                    VALUES(@Id, @Name, @AccountType, @Classification, @CurrentBalance, @SyncToken)
                                END";

                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@id", model.Id);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@AccountType", model.AccountType);
                cmd.Parameters.AddWithValue("@Classification", model.Classification);
                cmd.Parameters.AddWithValue("@CurrentBalance", model.CurrentBalance);
                cmd.Parameters.AddWithValue("@SyncToken", model.SyncToken);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Logger.WriteError(e, "Failed to connect to database.");
                    throw e;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public void  DeleteAccountInfo(string id)
        {
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                Logger.WriteDebug("Connecting to database server to delete account.");
                var qry = "DELETE FROM AccountInfo Where Id = @Id";
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Id", id);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Logger.WriteError(e, "Failed to connect to server to delete account.");
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