using Newtonsoft.Json;

namespace DiagnosticServicesModel.Request
{
    
    public class ParameterRequest
    {
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string? Value { get; set; }

        [JsonProperty("unit")]
        public string? Unit { get; set; }

        [JsonProperty("normalrange")]
        public string? Normalrange { get; set; }

        [JsonProperty("isCritical")]
        public bool IsCritical { get; set; }

        [JsonProperty("groupName")]
        public string? GroupName { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}