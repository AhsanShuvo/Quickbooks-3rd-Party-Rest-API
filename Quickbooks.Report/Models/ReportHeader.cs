using Newtonsoft.Json;
using System.Collections.Generic;

namespace Quickbooks.Report.Models
{
    public class ReportHeader
    {
        [JsonProperty("ReportName")]
        public string ReportName { get; set; }
        [JsonProperty("DateMacro")]
        public string DateMacro { get; set; }
        [JsonProperty("Currency")]
        public string Currency { get; set; }
        [JsonProperty("Time")]
        public string Time { get; set; }
        [JsonProperty("Option")]
        public List<Option> Option { get; set; }
    }

    public class Option
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Value")]
        public string Value { get; set; }
    }
}
