using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;

namespace QuickbooksApi.Repository
{
    public class ItemRepository : BaseRepository, IItemRepository
    {
        public void SaveItemInfo(ItemInfo model)
        {
            Logger.WriteDebug("Connecting to database server to insert/update iteminfo.");
            try
            {
                using (var ctx = new Entities())
                {
                    var id = model.Id;
                    if (ctx.ItemInfoes.Any(e => e.Id == id))
                    {
                        ctx.Entry(model).State = EntityState.Modified;
                    }
                    else
                    {
                        ctx.Entry(model).State = EntityState.Added;
                    }
                    ctx.SaveChanges();
                    Logger.WriteDebug("Saved item info successfully.");
                }
            }
            catch(Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database to save item info.");
                throw e;
            }
        }

        public void DeleteItem(string id)
        {
            Logger.WriteDebug("Connecting to database server to delete iteminfo.");
            try
            {
                using (var ctx = new Entities())
                {
                    var item = ctx.ItemInfoes.Find(id);
                    ctx.ItemInfoes.Attach(item);
                    ctx.ItemInfoes.Remove(item);
                    ctx.SaveChanges();
                }
                Logger.WriteDebug("Deleted item successfully.");
            }
            catch(Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database to delete item.");
            }
        }

        public void SaveCategoryInfo(ItemInfo model)
        {
            Logger.WriteDebug("Connecting to database server to insert/update iteminfo.");
            try
            {
                using (var ctx = new Entities())
                {
                    var id = model.Id;
                    if (ctx.CategoryInfoes.Any(e => e.Id == id))
                    {
                        ctx.Entry(model).State = EntityState.Modified;
                    }
                    else
                    {
                        ctx.Entry(model).State = EntityState.Added;
                    }
                    ctx.SaveChanges();
                    Logger.WriteDebug("Saved category info successfully.");
                }
            }
            catch(Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database to save category");
                throw e;
            }
        }
    }
}