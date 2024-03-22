using DiagnosticServicesModel.Request;
using System.Text.Json.Serialization;

namespace DiagnosticServicesModel.DataClass;

public class Test
{
    public int Id { get; set; }

    public Guid Appointmentid { get; set; }

    public string Laboratoryid { get; set; } = null!;

    public long Testid { get; set; }

    public string Name { get; set; } = null!;

    public string? Categorynote { get; set; }

    public decimal? Cost { get; set; }

    public decimal? Expenses { get; set; }

    public string[]? Result { get; set; }

    public string? Status { get; set; }

    public DateTime? Createdat { get; set; }

    [JsonIgnore]
    public virtual Appointment Appointment { get; set; } = null!;

    [JsonIgnore]
    public virtual Laboratory Laboratory { get; set; } = null!;

    [JsonIgnore]
    public virtual List<ParameterRequest> Parameters => Result?.Select(r => Newtonsoft.Json.JsonConvert.DeserializeObject<ParameterRequest>(r)).ToList() ?? new List<ParameterRequest>();
}
