using QuickbooksCommon.Logger;
using QuickbooksDAL.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;

namespace QuickbooksDAL.Repository
{
    public class ItemRepository : IItemRepository
    {
        public void SaveItemInfo(ItemInfo model)
        {
            Logger.WriteDebug("Connecting to database server to insert/update iteminfo.");
            try
            {
                using (var ctx = new QuickbooksEntities())
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
            catch (Exception e)
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
                using (var ctx = new QuickbooksEntities())
                {
                    var item = ctx.ItemInfoes.FirstOrDefault(x => x.Id == id);
                    if (item != null)
                    {
                        ctx.Entry(item).State = EntityState.Deleted;
                        ctx.SaveChanges();
                    }
                }
                Logger.WriteDebug("Deleted item successfully.");
            }
            catch (Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database to delete item.");
                throw e;
            }
        }

        public void SaveCategoryInfo(ItemInfo model)
        {
            Logger.WriteDebug("Connecting to database server to insert/update iteminfo.");
            try
            {
                using (var ctx = new QuickbooksEntities())
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
            catch (Exception e)
            {
                Logger.WriteError(e, "Failed to connect to the database to save category");
                throw e;
            }
        }
    }
}
