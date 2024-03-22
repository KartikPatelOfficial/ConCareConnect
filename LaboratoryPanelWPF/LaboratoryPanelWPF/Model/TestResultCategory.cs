using Newtonsoft.Json;
using System.Collections.Generic;

namespace LaboratoryPanelWPF.Model
{
    public enum ResultType { NORMAL, LOW, HIGH }

    public class TestResultCategory
    {
        public long Id { get; }

        public string Name { get; set; }

        public List<TestResultParameter> Results { get; set; }

        public string Note { get; private set; }

        public bool IsSelected { get; set; }

        public TestResultCategory(long id, string name, List<TestResultParameter> results, string note)
        {
            Id = id;
            Name = name;
            Results = results;
            Note = note;
            IsSelected = true;
        }
    }

    public class TestResultParameter
    {
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Result { get; set; }

        public string Unit { get; set; }
        public string NormalRange { get; set; }
        public ResultType ResultType { get; set; }

        public string GroupName { get; set; }

        [JsonIgnore]
        public bool IsSelected { get; set; }

        public TestResultParameter(string name, string result, string unit, string normalRange, ResultType resultType, string groupName)
        {
            Name = name;
            Result = result;
            Unit = unit;
            NormalRange = normalRange;
            ResultType = resultType;
            GroupName = groupName;
        }
    }
}
