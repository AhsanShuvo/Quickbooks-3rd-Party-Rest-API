using QuickbooksApi.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace QuickbooksApi.Repository
{
    public class ItemRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["QuickbooksDB"].ConnectionString;

        public void SaveItemInfo(ItemInfo model)
        {
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                var qry = @"IF EXISTS(SELECT * FROM ItemInfo WHERE Id = @Id)
                                BEGIN
                                    UPDATE ItemInfo
                                    SET Name = @Name, Type = @Type, Active = @Active, SyncToken = @SyncToken, UnitPrice = @UnitPrice,
                                    PurchaseCost = @PurchaseCost, QtyOnHand = @QtyOnHand
                                    WHERE Id = @Id
                                END
                            ELSE
                                BEGIN
                                    INSERT INTO ItemInfo(Id, Name, Type, Active, SyncToken, UnitPrice, PurchaseCost, QtyOnHand)
                                    VALUES(@Id, @Name, @Type, @Active, @SyncToken, @UnitPrice, @PurchaseCost, @QtyOnHand)
                                END";

                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@Type", model.Type);
                cmd.Parameters.AddWithValue("@Active", model.Active);
                cmd.Parameters.AddWithValue("@SyncToken", model.SyncToken);
                cmd.Parameters.AddWithValue("@UnitPrice", model.UnitPrice);
                cmd.Parameters.AddWithValue("@PurchaseCost", model.PurchaseCost);
                cmd.Parameters.AddWithValue("@QtyOnHand", model.QtyOnHand);

                
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch(Exception e)
                {
                    throw e;
                }
            }
        }

        public void DeleteItem(string id)
        {
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                var qry = @"IF EXISTS(SELECT * FROM ItemInfo WHERE Id = @Id)
                                BEGIN
                                    DELETE FROM ItemInfo WHERE Id = @Id
                                END";
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Id", id);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch(Exception e)
                {
                    throw e;
                }
            }
        }

        public void SaveCategoryInfo(CategoryInfo model)
        {
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                var qry = @"IF EXISTS(SELECT * FROM CategoryInfo WHERE Id = @Id)
                              BEGIN
                                UPDATE CategoryInfo
                                SET Name = @Name, Type = @Type, Active = @Active, SyncToken = @SyncToken
                                WHERE Id = @Id
                              END
                            ELSE
                              BEGIN
                                INSERT INTO CategoryInfo(Id, Name, Type, Active, SyncToken)
                                VALUES(@Id, @Name, @Type, @Active, @SyncToken)
                              END";

                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@Type", model.Type);
                cmd.Parameters.AddWithValue("@Active", model.Active);
                cmd.Parameters.AddWithValue("@SyncToken", model.SyncToken);

                
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch(Exception e)
                {
                    throw e;
                }
            }
        }
    }
}