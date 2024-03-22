using Newtonsoft.Json;

namespace LaboratoryPanelWPF.Model;

public class FirebaseLoginRequest
{
    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }

    [JsonProperty("returnSecureToken")]
    public bool ReturnSecureToken { get; set; } = true;

}

public class FirebaseLoginResponse { 
    
    [JsonProperty("idToken")]
    public string IdToken { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("refreshToken")]
    public string RefreshToken { get; set; }

    [JsonProperty("expiresIn")]
    public string ExpiresIn { get; set; }

    [JsonProperty("localId")]
    public string LocalId { get; set; }

    [JsonProperty("registered")]
    public bool Registered { get; set; }
}   
