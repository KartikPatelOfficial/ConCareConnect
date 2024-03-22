using System;
using System.Data.SQLite;

namespace LaboratoryPanelWPF.Model
{

    public enum TestFor
    {
        All,
        Gender
    }

    public class Parameter
    {
        public long TestId { get; set; }
        public string TestCode { get; }
        public string TestName { get; set; }
        public string PrintingName { get; }
        public TestFor TestFor { get; }
        public string DefaultValue { get; }
        public string Unit { get; }
        public string Formula { get; }
        public string TestGroupPrintingName { get; }

        public string CommonNormalRange { get; }
        public string MaleNormalRange { get; }
        public string FemaleNormalRange { get; }

        public double CommonLowerRange { get; }
        public double MaleLowerRange { get; }
        public double FemaleLowerRange { get; }

        public double CommonUpperRange { get; }
        public double MaleUpperRange { get; }
        public double FemaleUpperRange { get; }

        public Parameter(int testId, string testCode, string testName, string printingName, TestFor testFor, string defaultValue, string unit, string formula, string testGroupPrintingName, string commonNormalRange, string maleNormalRange, string femaleNormalRange, double commonLowerRange, double maleLowerRange, double femaleLowerRange, double commonUpperRange, double maleUpperRange, double femaleUpperRange)
        {
            TestId = testId;
            TestCode = testCode;
            TestName = testName;
            PrintingName = printingName;
            TestFor = testFor;
            DefaultValue = defaultValue;
            Unit = unit;
            Formula = formula;
            TestGroupPrintingName = testGroupPrintingName;
            CommonNormalRange = commonNormalRange;
            MaleNormalRange = maleNormalRange;
            FemaleNormalRange = femaleNormalRange;
            CommonLowerRange = commonLowerRange;
            MaleLowerRange = maleLowerRange;
            FemaleLowerRange = femaleLowerRange;
            CommonUpperRange = commonUpperRange;
            MaleUpperRange = maleUpperRange;
            FemaleUpperRange = femaleUpperRange;
        }

        public static Parameter FromReader(SQLiteDataReader reader, string testGroupPrintingName)
        {
            return new Parameter(
                Convert.ToInt32(reader["TestId"]),
                Convert.ToString(reader["TestCode"]),
                Convert.ToString(reader["TestName"]),
                Convert.ToString(reader["PrintingName"]),
                Convert.ToString(reader["TestFor"]) == "B" ? TestFor.Gender : TestFor.All,
                Convert.ToString(reader["DefaultValue"]),
                Convert.ToString(reader["Unit"]),
                Convert.ToString(reader["Formula"]),
                testGroupPrintingName,
                reader["CNormalRange"] == DBNull.Value ? null : Convert.ToString(reader["CNormalRange"]),
                reader["MNormalRange"] == DBNull.Value ? null : Convert.ToString(reader["MNormalRange"]),
                reader["FNormalRange"] == DBNull.Value ? null : Convert.ToString(reader["FNormalRange"]),
                reader["CLowerRange"] == DBNull.Value ? 0 : Convert.ToDouble(reader["CLowerRange"]),
                reader["MLowerRange"] == DBNull.Value ? 0 : Convert.ToDouble(reader["MLowerRange"]),
                reader["FLowerRange"] == DBNull.Value ? 0 : Convert.ToDouble(reader["FLowerRange"]),
                reader["CUpperRange"] == DBNull.Value ? 0 : Convert.ToDouble(reader["CUpperRange"]),
                reader["MUpperRange"] == DBNull.Value ? 0 : Convert.ToDouble(reader["MUpperRange"]),
                reader["FUpperRange"] == DBNull.Value ? 0 : Convert.ToDouble(reader["FUpperRange"])
            );
        }

        public static string TestForString(TestFor testFor)
        {
            return testFor switch
            {
                TestFor.All => "C",
                TestFor.Gender => "B",
                _ => throw new NotImplementedException()
            };
        }
    }
}