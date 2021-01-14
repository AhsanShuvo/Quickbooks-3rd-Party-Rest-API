using Newtonsoft.Json;
using System.Collections.Generic;

namespace Quickbooks.Report.Models
{
    public class ReportRow
    {
        [JsonProperty("Row")]
        public List<Row> Row { get; set; }
    }
    
    public class ColData
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
    
    public class Row
    {
        [JsonProperty("ColData")]
        public List<ColData> ColData { get; set; }
        [JsonProperty("Summary")]
        public ReportSummary Summary { get; set; }
        [JsonProperty("group")]
        public string Group { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    public class ReportSummary
    {
        [JsonProperty("ColData")]
        public List<ColData> ColData { get; set; }
    }
}
