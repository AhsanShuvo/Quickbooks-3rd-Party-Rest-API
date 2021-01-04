using Newtonsoft.Json;

namespace QuickbooksApi.Models
{
    public class EmployeeInfo : BaseModel
    {
        [JsonProperty("DisplayName")]
        public string DisplayName { get; set; }
        [JsonProperty("Active")]
        public bool Active { get; set; }
        [JsonProperty("GivenName")]
        public string GivenName { get; set; }
        [JsonProperty("FamilyName")]
        public string FamilyName { get; set; }
        [JsonProperty("SSN")]
        public string SSN { get; set; }

    }

    public class EmployeeApiModel : BaseApiModel
    {
        [JsonProperty("Employee")]
        public EmployeeInfo Employee { get; set; }
    }
}