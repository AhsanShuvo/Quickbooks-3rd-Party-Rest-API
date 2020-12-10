using System.Collections.Generic;

namespace QuickbooksApi.Models
{
    public class InvoiceInfo
    {
        public string Id { get; set; }
        public string TotalAmt { get; set; }
        public string SyncToken { get; set; }
        public CustomerReference CustomerRef { get; set; }
        public List<LineRef> Line { get; set; }
        public string TxnDate { get; set; }
    }
}