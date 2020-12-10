namespace QuickbooksApi.Models
{
    public class ItemInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double UnitPrice { get; set; }
        public double PurchaseCost { get; set; }
        public int QtyOnHand { get; set; }
        public string Type { get; set; }
        public bool Active { get; set; }
        public string SyncToken { get; set; }
        public IncomeAccount IncomeAccountRef { get; set; }
        public ExpenseAccount ExpenseAccountRef { get; set; }

    }
}