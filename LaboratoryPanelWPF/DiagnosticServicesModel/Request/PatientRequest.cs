using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticServicesModel.Request
{
    public class PatientRequest
    {

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [Required, EmailAddress]
        [JsonProperty("email")]
        public string? Email { get; set; }

        [Required, Phone]
        [JsonProperty("phone")]
        public string Phone { get; set; } = null!;

        [Required]
        [JsonProperty("address")]
        public string Address { get; set; } = null!;

        [JsonProperty("remarks")]
        public string? Remarks { get; set; }

        [JsonProperty("medicalHistory")]
        public string? Medicalhistory { get; set; }

        [Required]
        [JsonProperty("dob")]
        public DateTime? Dob { get; set; } = null!;

        [Required]
        [JsonProperty("gender")]
        public string Gender { get; set; } = null!;

        public PatientRequest(string name, string? email, string phone, string address, string? remarks, string? medicalhistory, DateTime? dob, string gender)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Email = email;
            Phone = phone ?? throw new ArgumentNullException(nameof(phone));
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Remarks = remarks;
            Medicalhistory = medicalhistory;
            Dob = dob ?? throw new ArgumentNullException(nameof(dob));
            Gender = gender ?? throw new ArgumentNullException(nameof(gender));
        }
    }

}
