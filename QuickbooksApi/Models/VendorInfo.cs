namespace QuickbooksApi.Models
{
    public class VendorInfo
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string CompanyName { get; set; }
        public bool Active { get; set; }
        public float Balance { get; set; }
        public string SyncToken { get; set; }
    }
}