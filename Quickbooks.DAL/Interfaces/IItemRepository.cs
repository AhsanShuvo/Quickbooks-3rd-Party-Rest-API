namespace QuickbooksDAL.Interfaces
{
    public interface IItemRepository
    {
        void SaveItemInfo(ItemInfo model);
        void DeleteItem(string id);
        void SaveCategoryInfo(ItemInfo model);
    }
}
