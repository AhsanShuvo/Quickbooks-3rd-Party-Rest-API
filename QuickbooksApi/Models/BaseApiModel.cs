using Newtonsoft.Json;

namespace QuickbooksApi.Models
{
    public class BaseApiModel
    {
        [JsonProperty("time")]
        public string Time { get; set; }
    }
}