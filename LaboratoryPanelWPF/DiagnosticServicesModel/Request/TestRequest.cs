using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticServicesModel.Request
{
    public class TestRequest
    {

        [Required]
        [JsonProperty("testName")]
        public string TestName { get; set; }


        [Required]
        [JsonProperty("testId")]
        public long TestId { get; set; }

        [Required]
        [JsonProperty("testPrice")]
        public decimal TestPrice { get; set; }

        [Required]
        [JsonProperty("testDescription")]
        public string TestDescription { get; set; }

        [Required]
        [JsonProperty("expenses")]
        public double Expenses { get; set; }

    }
}
