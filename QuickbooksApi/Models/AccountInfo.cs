namespace QuickbooksApi.Models
{
    public class AccountInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AccountType { get; set; }
        public string Classification { get; set; }
        public double CurrentBalance { get; set; }
        public string SyncToken { get; set; }
    }
}