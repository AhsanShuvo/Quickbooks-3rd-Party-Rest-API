using Newtonsoft.Json;

namespace Quickbooks.Report.Models
{
    public class QuickbooksReport
    {
        [JsonProperty("Header")]
        public ReportHeader Header { get; set; }
        [JsonProperty("Rows")]
        public ReportRow Rows { get; set; }
        [JsonProperty("Columns")]
        public Columns Columns { get; set; }
    }
}
