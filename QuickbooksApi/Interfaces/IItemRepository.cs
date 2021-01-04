using QuickbooksApi.Models;

namespace QuickbooksApi.Interfaces
{
    public interface IItemRepository
    {
        void SaveItemInfo(ItemInfo model);
        void DeleteItem(string id);
        void SaveCategoryInfo(ItemInfo model);
    }
}
