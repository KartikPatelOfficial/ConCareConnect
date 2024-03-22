using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DiagnosticServicesModel.DataClass;

public partial class Reference
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Cutoff { get; set; }

    public string Laboratoryid { get; set; } = null!;

    public string? Qualification { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Hospital { get; set; }

    public DateTime? Createdat { get; set; }

    [JsonIgnore]
    public virtual ICollection<Appointment> AppointmentDoctors { get; set; } = new List<Appointment>();

    [JsonIgnore]
    public virtual ICollection<Appointment> AppointmentSecondDoctors { get; set; } = new List<Appointment>();

    [JsonIgnore]
    public virtual Laboratory Laboratory { get; set; } = null!;
}
