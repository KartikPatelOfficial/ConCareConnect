using LaboratoryPanelWPF.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Windows;

namespace LaboratoryPanelWPF.Service
{
    public class DBService : DatabaseHelper, IDBService
    {

        public void UpdateProperty(string tableName, string property, dynamic value, string idKey, long id)
        {
            using var connection = GetNewConnection();

            var query = @$"
                    UPDATE {tableName} 
                    SET 
                    {property} = @value
                    Where 
                {idKey} = @id
                
                ";

            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@value", value);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        //Return categories
        //Table Name: MstCategory
        public List<Category> GetCategories()
        {
            try
            {

                using var connection = GetNewConnection();
                var categories = new List<Category>();
                using var command = new SQLiteCommand(SelectCategories, connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var category = new Category(
                        Convert.ToInt32(reader["CatagoryId"]),
                        Convert.ToString(reader["CatagoryName"]),
                        Convert.ToString(reader["PrintingName"]),
                        Convert.ToDouble(reader["Cost"]),
                        Convert.ToDouble(reader["LabCharge"]),
                        Convert.ToDouble(reader["Expenses"]),
                        Convert.ToString(reader["CatagoryNote"])
                    );
                    categories.Add(category);
                }

                return categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public List<Parameter> GetParameters()
        {
            using var connection = GetNewConnection();

            var tests = new List<Parameter>();
            using var sqLiteCommand = new SQLiteCommand(GetAllParameters, connection);
            var sqLiteDataReader = sqLiteCommand.ExecuteReader();
            while (sqLiteDataReader.Read())
            {
                tests.Add(Parameter.FromReader(sqLiteDataReader, null));
            }

            return tests;
        }

        //Return TestReport(Parameters)
        //@param categoryId = id of category
        public List<Parameter> GetParameters(long categoryId)
        {
            using var connection = GetNewConnection();
            var testGroups = new List<TestGroup>();
            var tests = new List<Parameter>();

            using var sqLiteCommand = new SQLiteCommand(SELECT_TESTS_GROUPS(categoryId), connection);
            var sqLiteDataReader = sqLiteCommand.ExecuteReader();
            while (sqLiteDataReader.Read())
            {
                testGroups.Add(TestGroup.FromReader(sqLiteDataReader));
            }

            foreach (var testGroup in testGroups)
            {
                using var sqLiteCommand2 = new SQLiteCommand(SELECT_TESTS(testGroup.Id), connection);
                var sqLiteDataReader2 = sqLiteCommand2.ExecuteReader();
                while (sqLiteDataReader2.Read())
                {
                    tests.Add(Parameter.FromReader(sqLiteDataReader2, testGroup.PrintingName));
                }
            }


            return tests;
        }


        //Return TestReport(Parameters)
        //@param testGroupId = id of test group
        public List<Parameter> GetParametersTest(long testGroupId)
        {
            using var connection = GetNewConnection();

            var tests = new List<Parameter>();
            using var sqLiteCommand = new SQLiteCommand(SELECT_TESTS(testGroupId), connection);
            var sqLiteDataReader = sqLiteCommand.ExecuteReader();
            while (sqLiteDataReader.Read())
            {
                tests.Add(Parameter.FromReader(sqLiteDataReader, null));
            }

            return tests;
        }

        //Return inserted test group
        //@param name: Name of test group
        //@param printingName: Printing name of test group
        public async Task<TestGroup> AddTestGroup(string name, string printingName)
        {
            using var connection = GetNewConnection();

            using var command = new SQLiteCommand(INSERT_TEST_GROUP(name, printingName), connection);
            var rows = await command.ExecuteNonQueryAsync();
            long lastId = connection.LastInsertRowId;
            TestGroup insertedGroup = new(lastId, name, printingName);

            return insertedGroup;
        }


        //Returns all test groups
        public async Task<List<TestGroup>> GetTestGroups()
        {
            using var connection = GetNewConnection();

            var testGroups = new List<TestGroup>();

            using var command = new SQLiteCommand(SelectAllTestsGroups, connection);
            var reader = await command.ExecuteReaderAsync();


            while (reader.Read())
            {
                testGroups.Add(TestGroup.FromReader(reader));
            }
            return testGroups;
        }

        //Returns test group by category id
        public List<TestGroup> GetTestGroups(long categoryId)
        {
            using var connection = GetNewConnection();
            var testGroups = new List<TestGroup>();

            using var command = new SQLiteCommand(SELECT_TESTS_GROUPS(categoryId), connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                testGroups.Add(TestGroup.FromReader(reader));
            }

            return testGroups;
        }

        //Return inserted test (parameter)
        public Parameter AddParameter(Parameter report, long testId)
        {
            using var connection = GetNewConnection();

            using var insertTestCommand = new SQLiteCommand(InsertParameter, connection);
            insertTestCommand.Parameters.AddWithValue("TestCode", report.TestCode);
            insertTestCommand.Parameters.AddWithValue("TestName", report.TestName);
            insertTestCommand.Parameters.AddWithValue("PrintingName", report.PrintingName);
            insertTestCommand.Parameters.AddWithValue("TestFor", Parameter.TestForString(report.TestFor));
            insertTestCommand.Parameters.AddWithValue("DefaultValue", report.DefaultValue);
            insertTestCommand.Parameters.AddWithValue("Unit", report.Unit);
            insertTestCommand.Parameters.AddWithValue("Formula", report.Formula);
            insertTestCommand.Parameters.AddWithValue("CNormalRange", report.CommonNormalRange);
            insertTestCommand.Parameters.AddWithValue("CLowerRange", report.CommonLowerRange);
            insertTestCommand.Parameters.AddWithValue("CUpperRange", report.CommonUpperRange);
            insertTestCommand.Parameters.AddWithValue("MNormalRange", report.MaleNormalRange);
            insertTestCommand.Parameters.AddWithValue("MLowerRange", report.MaleLowerRange);
            insertTestCommand.Parameters.AddWithValue("MUpperRange", report.MaleUpperRange);
            insertTestCommand.Parameters.AddWithValue("FNormalRange", report.FemaleNormalRange);
            insertTestCommand.Parameters.AddWithValue("FLowerRange", report.FemaleLowerRange);
            insertTestCommand.Parameters.AddWithValue("FUpperRange", report.FemaleUpperRange);
            insertTestCommand.ExecuteNonQuery();
            long id = connection.LastInsertRowId;
            report.TestId = id;
            InsertParameterInTestGroup(testId, id);

            return report;
        }

        public Parameter EditParameter(Parameter parameter)
        {
            using var connection = GetNewConnection();

            using var insertTestCommand = new SQLiteCommand(UpdateParameter, connection);
            insertTestCommand.Parameters.AddWithValue("@id", parameter.TestId);
            insertTestCommand.Parameters.AddWithValue("@testCode", parameter.TestCode);
            insertTestCommand.Parameters.AddWithValue("@testName", parameter.TestName);
            insertTestCommand.Parameters.AddWithValue("@printingName", parameter.PrintingName);
            insertTestCommand.Parameters.AddWithValue("@testFor", Parameter.TestForString(parameter.TestFor));
            insertTestCommand.Parameters.AddWithValue("@defaultValue", parameter.DefaultValue);
            insertTestCommand.Parameters.AddWithValue("@unit", parameter.Unit);
            insertTestCommand.Parameters.AddWithValue("@formula", parameter.Formula);
            insertTestCommand.Parameters.AddWithValue("@cNormalRange", parameter.CommonNormalRange);
            insertTestCommand.Parameters.AddWithValue("@cLowerRange", parameter.CommonLowerRange);
            insertTestCommand.Parameters.AddWithValue("@cUpperRange", parameter.CommonUpperRange);
            insertTestCommand.Parameters.AddWithValue("@mNormalRange", parameter.MaleNormalRange);
            insertTestCommand.Parameters.AddWithValue("@mLowerRange", parameter.MaleLowerRange);
            insertTestCommand.Parameters.AddWithValue("@mUpperRange", parameter.MaleUpperRange);
            insertTestCommand.Parameters.AddWithValue("@fNormalRange", parameter.FemaleNormalRange);
            insertTestCommand.Parameters.AddWithValue("@fLowerRange", parameter.FemaleLowerRange);
            insertTestCommand.Parameters.AddWithValue("@fUpperRange", parameter.FemaleUpperRange);
            insertTestCommand.ExecuteNonQuery();
            return parameter;
        }

        public void InsertParameterInTestGroup(long testGroupId, long testId)
        {
            using var connection = GetNewConnection();

            using var command = new SQLiteCommand(MergeTestgroupTest, connection);
            command.Parameters.AddWithValue("TestGroupId", testGroupId);
            command.Parameters.AddWithValue("TestId", testId);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public Category AddCategory(Category category)
        {
            using var connection = GetNewConnection();

            using var command = new SQLiteCommand(InsertCategory, connection);
            command.Parameters.AddWithValue("CatagoryName", category.Name);
            command.Parameters.AddWithValue("PrintingName", category.PrintingName);
            command.Parameters.AddWithValue("Cost", category.Cost);
            command.Parameters.AddWithValue("LabCharge", category.LabCharge);
            command.Parameters.AddWithValue("Expenses", category.Expenses);
            var rows = command.ExecuteNonQuery();
            long lastId = connection.LastInsertRowId;

            category.Id = lastId;

            return category;
        }

        public Category EditCategory(Category category)
        {
            using var connection = GetNewConnection();

            using var command = new SQLiteCommand(UpdateCategory, connection);
            command.Parameters.AddWithValue("@id", category.Id);
            command.Parameters.AddWithValue("@categoryName", category.Name);
            command.Parameters.AddWithValue("@printingName", category.PrintingName);
            command.Parameters.AddWithValue("@cost", category.Cost);
            command.Parameters.AddWithValue("@labcharge", category.LabCharge);
            command.Parameters.AddWithValue("@expenses", category.Expenses);
            command.ExecuteNonQuery();
            return category;
        }

        public void InsertTestGroupInCategory(long categoryId, long testGroupId)
        {
            using var connection = GetNewConnection();

            using var command = new SQLiteCommand(MergeCategoryTestgroup, connection);
            command.Parameters.AddWithValue("CatagoryId", categoryId);
            command.Parameters.AddWithValue("TestGroupId", testGroupId);

            command.ExecuteNonQuery();
            return;
        }

        //public void EditCostInCategory(long categoryId, double cost, string property)
        //{
        //    using var connection = GetNewConnection();

        //    var query = EDIT_COST_IN_CATEGORY;
        //    if (property == "@labcharge")
        //    {
        //        query = EDIT_LAB_CHARGES_IN_CATEGORY;
        //    }
        //    else if (property == "@expenses")
        //    {
        //        query = EDIT_EXPENSES_IN_CATEGORY;
        //    }

        //    using var command = new SQLiteCommand(query, connection);
        //    command.Parameters.AddWithValue("@id", categoryId);
        //    command.Parameters.AddWithValue(property, cost);

        //    command.ExecuteNonQuery();
        //    return;
        //}


        public void DeleteCategory(long categoryId)
        {
            using var connection = GetNewConnection();
            using var command = new SQLiteCommand("DELETE FROM MstCatagory WHERE CatagoryId = " + categoryId, connection);
            command.ExecuteNonQuery();
            return;
        }

        public void DeleteTestGroup(long testGroupId)
        {
            using var connection = GetNewConnection();
            using var command = new SQLiteCommand("DELETE FROM MstTestGroup WHERE TestGroupId = " + testGroupId, connection);
            command.ExecuteNonQuery();
            return;
        }

        public void DeleteParameter(long parameterId)
        {
            using var connection = GetNewConnection();
            using var command = new SQLiteCommand("DELETE FROM MstTest WHERE TestId = " + parameterId, connection);
            command.ExecuteNonQuery();
            return;
        }

        public void RemoveTestGroupFromCategory(long categoryId, long testGroupId)
        {
            using var connection = GetNewConnection();
            using var command = new SQLiteCommand("DELETE FROM MstCatagoryTestGroup WHERE CatagoryId = " + categoryId + " AND TestGroupId = " + testGroupId, connection);
            command.ExecuteNonQuery();
            return;
        }

        public async Task UpdateTestGroup(long testGroupId, string name, string printingName)
        {
            using var connection = GetNewConnection();
            using var command = new SQLiteCommand("UPDATE MstTestGroup SET TestGroupName = '" + name + "', PrintingName = '" + printingName + "' WHERE TestGroupId = " + testGroupId, connection);
            await command.ExecuteScalarAsync();
            return;
        }
    }

}
