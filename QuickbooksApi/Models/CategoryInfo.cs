namespace QuickbooksApi.Models
{
    public class CategoryInfo : BaseModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Active { get; set; }
    }
}