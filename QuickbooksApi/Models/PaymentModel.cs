using Newtonsoft.Json;

namespace QuickbooksWeb.Models
{
    public class PaymentModel : BaseModel
    {
        [JsonProperty("TotalAmt")]
        public double TotalAmt { get; set; }
        [JsonProperty("CustomerRef")]
        public CustomerReference CustomerRef { get; set; }
        [JsonProperty("UnappliedAmt")]
        public double UnappliedAmt { get; set; }
        [JsonProperty("TxnDate")]
        public string TxnDate { get; set; }
    }

    public class PaymentApiModel : BaseApiModel
    {
        [JsonProperty("Payment")]
        public PaymentModel Payment { get; set; }
    }
}