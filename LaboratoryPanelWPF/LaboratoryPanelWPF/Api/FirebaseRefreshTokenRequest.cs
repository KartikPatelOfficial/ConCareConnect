using Newtonsoft.Json;

namespace LaboratoryPanelWPF.Api;

public class FirebaseRefreshTokenRequest
{
    [JsonProperty("grant_type")] public string GrantType { get; set; } = "refresh_token";

    [JsonProperty("refresh_token")] public string RefreshToken { get; set; }

    public FirebaseRefreshTokenRequest(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
}