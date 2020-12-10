namespace QuickbooksApi.Models
{
    public class PaymentInfo
    {
        public string Id { get; set; }
        public double TotalAmt { get; set; }
        public CustomerReference CustomerRef { get; set; }
        public string SyncToken { get; set; }
    }
}