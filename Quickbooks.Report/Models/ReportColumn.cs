using Newtonsoft.Json;
using System.Collections.Generic;

namespace Quickbooks.Report.Models
{
    public class Column
    {
        [JsonProperty("ColType")]
        public string ColumnType { get; set; }
        [JsonProperty("ColTitle")]
        public string ColumnTitle { get; set; }
    }

    public class Columns
    {
        [JsonProperty("Column")]
        public List<Column> ReportColumns { get; set; }
    }
}
