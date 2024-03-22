using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DiagnosticServicesModel.DataClass;

public class Laboratory
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Logo { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateTime? Createdat { get; set; }

    public string? Status { get; set; }

    public string? Sign { get; set; }

    public string Addressline1 { get; set; } = null!;

    public string Addressline2 { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Pincode { get; set; } = null!;

    public string Firebaseuid { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    [JsonIgnore]
    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();

    [JsonIgnore]
    public virtual ICollection<Reference> References { get; set; } = new List<Reference>();

    [JsonIgnore]
    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
