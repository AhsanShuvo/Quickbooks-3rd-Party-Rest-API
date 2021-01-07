using Newtonsoft.Json;

namespace QuickbooksWeb.Models
{
    public class BaseApiModel
    {
        [JsonProperty("time")]
        public string Time { get; set; }
    }
}