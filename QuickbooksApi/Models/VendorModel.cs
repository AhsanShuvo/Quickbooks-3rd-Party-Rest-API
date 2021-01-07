namespace QuickbooksWeb.Models
{
    public class VendorModel : BaseModel
    {
        public string DisplayName { get; set; }
        public string CompanyName { get; set; }
        public bool Active { get; set; }
        public float Balance { get; set; }
    }

    public class VendorApiModel : BaseApiModel
    {
        public VendorModel Vendor { get; set; }
    }
}