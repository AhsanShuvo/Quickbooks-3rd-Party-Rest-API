using Newtonsoft.Json;

namespace QuickbooksApi.Models
{
    public class ItemInfo : BaseModel
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("UnitPrice")]
        public double UnitPrice { get; set; }
        [JsonProperty("PurchaseCost")]
        public double PurchaseCost { get; set; }
        [JsonProperty("QtyOnHand")]
        public int QtyOnHand { get; set; }
        [JsonProperty("Type")]
        public string Type { get; set; }
        [JsonProperty("Active")]
        public bool Active { get; set; }
        [JsonProperty("IncomeAccountRef")]
        public IncomeAccount IncomeAccountRef { get; set; }
        [JsonProperty("ExpenseAccountRef")]
        public ExpenseAccount ExpenseAccountRef { get; set; }
        [JsonProperty("FullyQualifiedName")]
        public string FullyQualifiedName { get; set; }
        [JsonProperty("InvStartDate")]
        public string InvStartDate { get; set; }

    }

    public class ItemApiModel : BaseApiModel
    {
        [JsonProperty("Item")]
        public ItemInfo Item { get; set; }
    }
}