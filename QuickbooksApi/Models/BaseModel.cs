using Newtonsoft.Json;

namespace QuickbooksApi.Models
{
    public class BaseModel
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("SyncToken")]
        public string SyncToken { get; set; }
        [JsonProperty("domain")]
        public string Domain { get; set; }
    }
}
