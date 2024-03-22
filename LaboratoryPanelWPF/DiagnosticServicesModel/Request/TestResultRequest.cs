using Newtonsoft.Json;

namespace DiagnosticServicesModel.Request
{
    public class TestResultRequest
    {
        [JsonProperty("testid")]
        public long Testid { get; set; }

        [JsonProperty("parameters")]
        public List<ParameterRequest> Parameters { get; set; }



    }
}
