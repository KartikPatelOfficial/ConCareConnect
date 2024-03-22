using System;
using System.Data.Common;

namespace LaboratoryPanelWPF.Model
{
    public class TestGroup
    {
        private string groupName;
        private string printingName;

        public long Id { get; }
        public string GroupName
        {
            get => groupName;
            set
            {
                groupName = value;
            }
        }
        public string PrintingName
        {
            get => printingName;
            set
            {
                printingName = value;
            }
        }
        public TestGroup(long id, string groupName, string printingName)
        {
            Id = id;
            this.groupName = groupName;
            this.printingName = printingName;
        }

        public static TestGroup FromReader(DbDataReader reader)
        {
            var testGroupId = 0;
            if (reader["TestGroupId"] != DBNull.Value)
            {
                testGroupId = Convert.ToInt32(reader["TestGroupId"]);
            }
            return new TestGroup(
                testGroupId,
                Convert.ToString(reader["TestGroupName"]),
                Convert.ToString(reader["PrintingName"])
            );
        }
    }
}
