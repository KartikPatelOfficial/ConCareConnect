using Newtonsoft.Json;

namespace LaboratoryPanelWPF.Model
{
    public class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProfileName { get; set; }
        public string Code { get; set; }
        public int Price { get; set; }

        public TestModel(int id, string name, string profileName, string code, int price)
        {
            Id = id;
            Name = name;
            ProfileName = profileName;
            Code = code;
            Price = price;
        }
    }

    public class TestResponse
    {
        [JsonProperty("_id")]
        public string Id;

        [JsonProperty("name")]
        public string Name;
    }

    public class TestInvoice
    {
        [JsonProperty("subTotal")]
        public int Subtotal;

        [JsonProperty("discount")]
        public int Discount;

        [JsonProperty("grandTotal")]
        public int GrandTotal;

        public TestInvoice(int subtotal, int discount, int grandTotal)
        {
            Subtotal = subtotal;
            Discount = discount;
            GrandTotal = grandTotal;
        }
    }
}