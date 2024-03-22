using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticServicesModel.Request
{
    public class AppointmentRequest
    {

        [Required]
        [JsonProperty("patientId")]
        public Guid Patientid { get; set; }

        [Required]
        [JsonProperty("doctorId")]
        public Guid Doctorid { get; set; }

        [JsonProperty("secondDoctorid")]
        public Guid? SecondDoctorid { get; set; }

        [Required]
        [JsonProperty("sampleType")]
        public string Sampletype { get; set; } = null!;

        [Required]
        [JsonProperty("patientType")]
        public string Patienttype { get; set; } = null!;

        [Required]
        [JsonProperty("total")]
        public decimal Total { get; set; }

        [Required]
        [JsonProperty("labCharges")]
        public decimal Labcharges { get; set; }

        [Required]
        [JsonProperty("expenses")]
        public decimal Expenses { get; set; }

        [Required]
        [JsonProperty("doctorMargin")]
        public decimal? Doctormargin { get; set; }

        [Required]
        [JsonProperty("paymentStatus")]
        public string? Paymentstatus { get; set; }

        [Required]
        [JsonProperty("tests")]
        public List<TestRequest> Tests { get; set; } = null!;
    }

}
