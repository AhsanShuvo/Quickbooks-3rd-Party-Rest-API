namespace QuickbooksApi.Models
{
    public class VendorInfo : BaseModel
    {
        public string DisplayName { get; set; }
        public string CompanyName { get; set; }
        public bool Active { get; set; }
        public float Balance { get; set; }
    }

    public class VendorApiModel : BaseApiModel
    {
        public VendorInfo Vendor { get; set; }
    }
}