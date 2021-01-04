using QuickbooksApi.Models;

namespace QuickbooksApi.Interfaces
{
    public interface IUserRepository
    {
        void SaveUserInfo(UserInfo user);
        UserInfo GetUserInfo(string realmId);
    }
}
