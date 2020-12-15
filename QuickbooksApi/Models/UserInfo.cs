using System;

namespace QuickbooksApi.Models
{
    public class UserInfo
    {
        public string RealmId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiresIn { get; set; }
        public DateTime RefreshTokenExpiresIn { get; set; }
    }
}