using Newtonsoft.Json;

namespace QuickbooksWeb.Models
{
    public class EmployeeModel : BaseModel
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
        public EmployeeModel Employee { get; set; }
    }
}