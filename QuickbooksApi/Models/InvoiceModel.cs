using System.Collections.Generic;

namespace QuickbooksApi.Models
{
    public class InvoiceModel : BaseModel
    {
        public double TotalAmt { get; set; }
        public CustomerReference CustomerRef { get; set; }
        public List<LineRef> Line { get; set; }
        public string TxnDate { get; set; }
    }

    public class InvoiceApiModel : BaseApiModel
    {
        public InvoiceModel Invoice { get; set; }
    }
}