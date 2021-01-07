namespace QuickbooksDAL.Interfaces
{
    public interface IUserRepository
    {
        void SaveUserInfo(UserInfo user);
        UserInfo GetUserInfo(string realmId);
    }
}
