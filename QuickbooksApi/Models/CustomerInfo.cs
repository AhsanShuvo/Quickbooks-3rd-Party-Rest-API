namespace QuickbooksApi.Models
{
    public class CustomerInfo
    {
        public string Id { get; set; }
        public double Balance { get; set; }
        public bool Active { get; set; }
        public string CompanyName { get; set; }
        public string DisplayName { get; set; }
        public string SyncToken { get; set; }
    }
}