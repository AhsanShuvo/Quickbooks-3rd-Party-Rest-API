using QuickbooksApi.Models;

namespace QuickbooksApi.Interfaces
{
    public interface IAccountRepository
    {
        void SaveAccountInfo(AccountInfo model);
        void DeleteAccountInfo(string id);
    }
}
