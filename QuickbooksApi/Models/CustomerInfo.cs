using Newtonsoft.Json;

namespace QuickbooksApi.Models
{
    public class CustomerInfo : BaseModel
    {
        [JsonProperty("Balance")]
        public double Balance { get; set; }
        [JsonProperty("Active")]
        public bool Active { get; set; }
        [JsonProperty("CompanyName")]
        public string CompanyName { get; set; }
        [JsonProperty("DisplayName")]
        public string DisplayName { get; set; }
        [JsonProperty("GivenName")]
        public string GivenName { get; set; }
        [JsonProperty("FullyQualifiedName")]
        public string FullyQualifiedName { get; set; }
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("Job")]
        public bool Job { get; set; }
        [JsonProperty("BalanceWithJobs")]
        public string BalanceWithJobs { get; set; }
        [JsonProperty("MiddleName")]
        public string MiddleName { get; set; }
    }

    public class CustomerApiModel : BaseApiModel
    {
        [JsonProperty("Customer")]
        public CustomerInfo Customer { get; set; }
    }
}