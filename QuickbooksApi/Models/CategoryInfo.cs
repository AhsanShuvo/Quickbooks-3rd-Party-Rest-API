namespace QuickbooksApi.Models
{
    public class CategoryInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Active { get; set; }
        public string SyncToken { get; set; }
    }
}