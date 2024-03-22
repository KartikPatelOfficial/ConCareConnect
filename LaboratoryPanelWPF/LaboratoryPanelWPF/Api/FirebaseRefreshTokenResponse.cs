using Newtonsoft.Json;

namespace LaboratoryPanelWPF.Api
{
    public class FirebaseRefreshTokenResponse
    {

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("refresh_token")]

        public string RefreshToken { get; set; }

        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId
        {
            get; set;

        }

    }
}