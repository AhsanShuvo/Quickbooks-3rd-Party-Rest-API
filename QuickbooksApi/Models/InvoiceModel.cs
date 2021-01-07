using System.Collections.Generic;

namespace QuickbooksWeb.Models
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