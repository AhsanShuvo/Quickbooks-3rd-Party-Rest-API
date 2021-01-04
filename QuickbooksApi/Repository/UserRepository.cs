using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using QuickbooksApi.Models;
using System;
using System.Data.SqlClient;

namespace QuickbooksApi.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public void SaveUserInfo(UserInfo user)
        {
            Logger.WriteDebug("Connecting to database server to insert/update userinfo.");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                var qry = @"IF EXISTS(SELECT * FROM UserInfo WHERE RealmId = @RealmId)
                                BEGIN
                                    UPDATE UserInfo
                                    SET AccessToken = @AccessToken, RefreshToken=@RefreshToken, AccessTokenExpiresIn=@AccessTokenExpiresIn, RefreshTokenExpiresIn=@RefreshTokenExpiresIn
                                    WHERE RealmId = @RealmId
                                END
                            ELSE
                                BEGIN
                                    INSERT INTO UserInfo(RealmId, AccessToken, RefreshToken, AccessTokenExpiresIn, RefreshTokenExpiresIn)
                                    VALUES(@RealmId, @AccessToken, @RefreshToken, @AccessTokenExpiresIn, @RefreshTokenExpiresIn)
                                END";

                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@RealmId", user.RealmId);
                cmd.Parameters.AddWithValue("@AccessToken", user.AccessToken);
                cmd.Parameters.AddWithValue("@RefreshToken", user.RefreshToken);
                cmd.Parameters.AddWithValue("@AccessTokenExpiresIn", user.AccessTokenExpiresIn);
                cmd.Parameters.AddWithValue("@RefreshTokenExpiresIn", user.RefreshTokenExpiresIn);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Logger.WriteError(e, "Failed to connect to database to insert/update userinfo.");
                    throw e;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public UserInfo GetUserInfo(string realmId)
        {
            Logger.WriteDebug("Connecting to database server to get userinfo.");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                var qry = "SELECT * FROM UserInfo WHERE RealmId = @RealmId";

                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@RealmId", realmId);
                UserInfo user = new UserInfo();
                try
                {
                    con.Open();
                    SqlDataReader rd = cmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            user.RealmId = rd.GetString(0);
                            user.AccessToken = rd.GetString(1);
                            user.RefreshToken = rd.GetString(2);
                            user.AccessTokenExpiresIn = rd.GetDateTime(3);
                            user.RefreshTokenExpiresIn = rd.GetDateTime(4);
                        }
                    }
                }
                catch(Exception e)
                {
                    Logger.WriteError(e, "Failed to connect to database to get userinfo.");
                    throw e;
                }
                finally
                {
                    con.Close();
                }
                return user;
            }
        }
    }
}