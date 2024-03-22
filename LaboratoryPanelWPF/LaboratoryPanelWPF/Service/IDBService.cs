using LaboratoryPanelWPF.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaboratoryPanelWPF.Service
{
    public interface IDBService
    {

        public void UpdateProperty(string tableName, string property, dynamic value, string idKey, long id);

        //Add all functions from DBService.cs

        public List<Category> GetCategories();

        public List<Parameter> GetParameters();

        public List<Parameter> GetParameters(long categoryId);

        public List<Parameter> GetParametersTest(long testId);

        public Task<TestGroup> AddTestGroup(string name, string printingName);

        public Task<List<TestGroup>> GetTestGroups();

        public List<TestGroup> GetTestGroups(long categoryId);

        public Parameter AddParameter(Parameter parameter, long testId);

        public Parameter EditParameter(Parameter parameter);

        public void InsertParameterInTestGroup(long testGroupId, long parameterId);

        public Category AddCategory(Category category);

        public Category EditCategory(Category category);

        public void InsertTestGroupInCategory(long categoryId, long testGroupId);

        public void RemoveTestGroupFromCategory(long categoryId, long testGroupId);

        public Task UpdateTestGroup(long testGroupId, string name, string printingName);

        public void DeleteCategory(long categoryId);

        public void DeleteTestGroup(long testGroupId);

        public void DeleteParameter(long parameterId);



    }
}
