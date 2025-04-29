using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BackEnd_Server.Models
{
    public class ScanRule
    {
        public string? RuleId { get; set; }
        public string? Pattern { get; set; }
        public string? Description { get; set; }

        [JsonIgnore]
        public Regex? Compiled { get; set; }
    }
    public class ScanResult
    {
        public string? FilePath { get; set; }
        public int LineNumber { get; set; }
        public string? Snippet { get; set; }
        public string? RuleId { get; set; }
        public string? Description { get; set; }
    }
}