using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LaboratoryPanelWPF.Model
{
    public class TestHistoryRequest
    {
        [JsonProperty("patientId")]
        string PatientId { get; set; }

        [JsonProperty("testIds")]
        List<long> TestIds { get; set; }

        public TestHistoryRequest(string patientId, List<long> testIds)
        {
            PatientId = patientId;
            TestIds = testIds;
        }
    }

    public class TestHistoryResponse
    {
        [JsonProperty("testId")]
        public long TestId { get; set; }

        [JsonProperty("data")]
        public List<TestHistoryData> Data { get; set; }

    }

    public class TestHistoryData
    {
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("result")]
        public List<TestHistoryResult> Results { get; set; }

        [JsonIgnore]
        public bool IsSelected { get; set; }

        TestHistoryData()
        {
            IsSelected = true;
        }
    }

    public class TestHistoryResult
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
