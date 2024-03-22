using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticServicesModel.DataClass
{
    public class LoginRequest
    {
        [Required, EmailAddress]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Required, MinLength(8)]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}