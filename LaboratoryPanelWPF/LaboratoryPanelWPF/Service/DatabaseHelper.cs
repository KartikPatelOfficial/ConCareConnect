using System;
using System.Data.SQLite;

namespace LaboratoryPanelWPF.Service
{
    public abstract class DatabaseHelper
    {
        private static readonly string DATABASE_PATH = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "laboratory.sqlite");

        public static string CategoryTable = "MstCatagory";
        public static string CategorySetTable = "MstCategorySet";
        public static string CategorySubTable = "MstCategorySub";
        public static string CategorySubSetTable = "MstCategorySubSet";
        public static string TestTable = "MstTest";
        public static string TestGroupTable = "MstTestGroup";
        public static string TestGroupSubTable = "MstTestGroupSub";
        public static string ProfileNoteTable = "MstProfileNote";

        protected static SQLiteConnection GetNewConnection() => new SQLiteConnection($"Data Source={DATABASE_PATH};").OpenAndReturn();

        protected static string INSERT_TEST_GROUP(string name, string printingName) => $"INSERT INTO MstTestGroup (TestGroupName, PrintingName) VALUES ('{name}', '{printingName}');";


        public static string UpdateTestGroup => "UPDATE MstTestGroup SET TestGroupName = @testGroupName, PrintingName = @printingName WHERE TestGroupId = @id";

        public static string InsertCategory => @"
                    INSERT INTO MstCatagory(
                        CatagoryName,
                        PrintingName,
                        Cost,
                        LabCharge,
                        Expenses
                    ) VALUES(?,?,?,?,?)
                ";

        public static string UpdateCategory => @"
                    UPDATE MstCatagory 
                    SET 
                    CatagoryName = @categoryName, 
                    PrintingName = @printingName,
                    Cost = @cost,
                    LabCharge = @labcharge,
                    Expenses = @expenses 
                    Where CatagoryId = @id
                ";

        public static string EditProperty => @"
                    UPDATE @tableName 
                    SET 
                    @property = @value
                    Where @idKey = @id
                ";

        public static string EditCategory => @"
                    UPDATE MstCatagory 
                    SET 
                    CatagoryNote = @note
                    Where CatagoryId = @id
                ";

        public static string SelectAllTestsGroups = @$"
                    SELECT DISTINCT testgroup.TestGroupId, testgroup.TestGroupName, testgroup.PrintingName
                    FROM MstTestGroup testgroup
                ";

        public static string SELECT_TESTS_GROUPS(long categoryId) => @$"
                    SELECT DISTINCT testgroup.TestGroupId, testgroup.TestGroupName, testgroup.PrintingName
                    FROM MstTestGroup testgroup
                    JOIN MstCatagorySub categorysub ON testgroup.TestGroupId = categorysub.TestGroupId
                    WHERE categorysub.CatagoryId = {categoryId};
                ";

        public static string SELECT_TESTS(long testGroupId) => $@"
                    SELECT DISTINCT 
                    test.TestId, test.TestCode, test.TestName, test.PrintingName, test.TestFor, test.DefaultValue, test.Unit, test.Formula,
                    test.CNormalRange, test.MNormalRange, test.FNormalRange,
                    test.CLowerRange, test.MLowerRange, test.FLowerRange,
                    test.CUpperRange, test.MUpperRange, test.FUpperRange 
                    FROM MstTest test
                    JOIN MstTestGroupSub testgroupsub ON test.TestId = testgroupsub.TestId
                    WHERE testgroupsub.TestGroupId = {testGroupId};

                ";

        public static string SelectCategories = "SELECT * FROM MstCatagory";

        public static string SelectPatientTypes = "SELECT * FROM PatientTypes";

        public static string GetAllParameters = "SELECT * FROM MstTest";

        public static string MergeTestgroupTest => "INSERT INTO MstTestGroupSub(TestGroupId, TestId) VALUES (?,?)";

        public static string MergeCategoryTestgroup => "INSERT INTO MstCatagorySub(CatagoryId, TestGroupId) VALUES (?,?)";

        public static string InsertParameter => $@"
                    INSERT INTO MstTest (
                        TestCode,
                        TestName,
                        PrintingName,
                        TestFor,
                        DefaultValue,
                        Unit,
                        Formula,
                        CNormalRange,
                        CLowerRange,
                        CUpperRange,
                        MNormalRange,
                        MLowerRange,
                        MUpperRange,
                        FNormalRange,
                        FLowerRange,
                        FUpperRange
                    ) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)
                ";

        public static string UpdateParameter => $@"
                    UPDATE MstTest 
                    SET
                    TestCode = @testCode,
                    TestName = @testName,
                    PrintingName = @printingName,
                    TestFor = @testFor,
                    DefaultValue = @defaultValue,
                    Unit = @unit,
                    Formula = @formula,
                    CNormalRange = @cNormalRange,
                    CLowerRange = @cLowerRange,
                    CUpperRange = @cUpperRange,
                    MNormalRange = @mNormalRange,
                    MLowerRange = @mLowerRange,
                    MUpperRange = @mUpperRange,
                    FNormalRange = @fNormalRange,
                    FLowerRange = @fLowerRange,
                    FUpperRange = @fUpperRange
                    WHERE TestId = @id
                ";

    }
}
