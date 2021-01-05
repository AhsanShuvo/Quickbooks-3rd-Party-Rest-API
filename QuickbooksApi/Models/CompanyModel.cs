using Newtonsoft.Json;

namespace QuickbooksApi.Models
{
    public class CompanyModel : BaseModel
    {
        [JsonProperty("CompanyName")]
        public string CompanyName { get; set; }
        [JsonProperty("CompanyStartDate")]
        public string CompanyStartDate { get; set; }
        [JsonProperty("Country")]
        public string Country { get; set; }
        [JsonProperty("FiscalYearStartMonth")]
        public string FiscalYearStartMonth { get; set; }
        [JsonProperty("LegalName")]
        public string LegalName { get; set; }


    }

    public class CompanyApiModel : BaseApiModel
    {
        [JsonProperty("CompanyInfo")]
        public CompanyModel CompanyInfo;
    }
}