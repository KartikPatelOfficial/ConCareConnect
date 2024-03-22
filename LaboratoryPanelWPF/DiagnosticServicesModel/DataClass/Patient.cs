using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DiagnosticServicesModel.DataClass;

public class Patient
{
    [Key]
    public Guid Id { get; set; }

    public string Laboratoryid { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Remarks { get; set; }

    public string? Medicalhistory { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Dob { get; set; } = null!;

    public string Gender { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    [JsonIgnore]
    public virtual Laboratory Laboratory { get; set; } = null!;
}
