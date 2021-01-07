namespace QuickbooksWeb.Models
{
    public class CategoryModel : BaseModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Active { get; set; }
    }
}