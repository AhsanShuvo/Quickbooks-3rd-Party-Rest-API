using Newtonsoft.Json;
using System.Collections.Generic;


namespace QuickbooksApi.Models
{
    public class WebhookModel
    {
        [JsonProperty("eventNotifications")]
        public List<EventNotification> EventNotifications { get; set; }
    }

    public class EventNotification
    {
        [JsonProperty("realmId")]
        public string RealmId { get; set; }
        [JsonProperty("dataChangeEvent")]
        public DatachangeEvent DataChangeEvent { get; set; }
    }

    public class DatachangeEvent
    {
        [JsonProperty("entities")]
        public List<Entity> Entities { get; set; }
    }

    public class Entity
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("operation")]
        public string Operation { get; set; }
    }
}