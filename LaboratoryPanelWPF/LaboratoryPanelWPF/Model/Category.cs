using Newtonsoft.Json;
using System.Collections.Generic;

namespace LaboratoryPanelWPF.Model
{
    public class Category
    {

        private string name;
        private string pritingName;
        private double cost;
        private double labCharge;
        private double expenses;


        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("_id")]
        public string DatabaseId { get; set; }

        [JsonProperty("name")]
        public string Name
        {
            get => name; set
            {
                name = value;
            }
        }

        [JsonProperty("printingName")]
        public string PrintingName
        {
            get => pritingName; set
            {
                pritingName = value;
            }
        }

        [JsonProperty("cost")]
        public double Cost
        {
            get => cost;
            set
            {
                cost = value;
            }
        }

        [JsonProperty("labCharge")]
        public double LabCharge
        {
            get => labCharge;
            set
            {
                labCharge = value;
            }
        }

        [JsonProperty("expenses")]
        public double Expenses
        {
            get => expenses;
            set
            {
                expenses = value;
            }
        }

        [JsonProperty("result")]
        public List<TestResultParameter> Result { get; set; }

        [JsonProperty("categoryNote")]
        public string CategoryNote { get; set; }

        [JsonConstructor]
        public Category(long id, string name, string printingName, double cost, double labCharge, double expenses, string note)
        {
            Id = id;
            Name = name;
            PrintingName = printingName;
            Cost = cost;
            LabCharge = labCharge;
            Expenses = expenses;
            CategoryNote = note;
        }

        public Category(long id, string name, string printingName, double cost, double labCharge, double expenses)
        {
            Id = id;
            Name = name;
            PrintingName = printingName;
            Cost = cost;
            LabCharge = labCharge;
            Expenses = expenses;
        }

        public Category()
        {
            
        }
    }
}
