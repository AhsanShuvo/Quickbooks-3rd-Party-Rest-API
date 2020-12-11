using QuickbooksApi.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace QuickbooksApi.Repository
{
    public class AccountRepository
    {
        public void SaveAccountInfo(AccountInfo model)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["QuickbooksDB"].ConnectionString;
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