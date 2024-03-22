using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DiagnosticServicesModel.Request
{
    public class LaboratoryRequest
    {
        [Required]
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [Required, EmailAddress]
        [JsonProperty("email")]
        public string Email { get; set; } = null!;

        [Required, MinLength(8)]
        [JsonProperty("password")]
        public string Password { get; set; } = null!;

        [Required]
        [JsonProperty("logo")]
        public string Logo { get; set; } = null!;

        [Required]
        [JsonProperty("phone")]
        public string Phone { get; set; } = null!;

        [Required]
        [JsonProperty("sign")]
        public string Sign { get; set; } = null!;

        [Required]
        [JsonProperty("addressLine1")]
        public string AddressLine1 { get; set; } = null!;

        [Required]
        [JsonProperty("addressLine2")]
        public string AddressLine2 { get; set; } = null!;

        [Required]
        [JsonProperty("city")]
        public string City { get; set; } = null!;

        [Required, RegularExpression(@"^[1-9][0-9]{5}$")]
        [JsonProperty("pincode")]
        public string Pincode { get; set; } = null!;

    }
}
