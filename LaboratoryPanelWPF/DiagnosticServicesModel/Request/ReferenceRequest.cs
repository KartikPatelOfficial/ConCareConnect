using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticServicesModel.Request
{
    public class ReferenceRequest
    {

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [Required]
        [JsonProperty("cutoff")]
        public decimal Cutoff { get; set; }

        [JsonProperty("qualification")]
        public string? Qualification { get; set; }

        [JsonProperty("address")]
        public string? Address { get; set; }

        [JsonProperty("phone")]
        public string? Phone { get; set; }

        [JsonProperty("hospital")]
        public string? Hospital { get; set; }
    }
}
