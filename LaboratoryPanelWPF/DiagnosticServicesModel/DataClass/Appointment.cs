using System.ComponentModel.DataAnnotations;

namespace DiagnosticServicesModel.DataClass;

public class Appointment
{
    [Key]
    public Guid Id { get; set; }

    public Guid Patientid { get; set; }

    public string Laboratoryid { get; set; } = null!;

    public Guid Doctorid { get; set; }

    public Guid? SecondDoctorid { get; set; }

    public string Sampletype { get; set; } = null!;

    public string Patienttype { get; set; } = null!;

    public DateTime? Createdat { get; set; }

    public DateTime? Reportedat { get; set; }

    public string? Status { get; set; }

    public decimal Total { get; set; }

    public decimal Labcharges { get; set; }

    public decimal Expenses { get; set; }

    public decimal? Doctormargin { get; set; }

    public string? Paymentstatus { get; set; }

    public virtual Reference Doctor { get; set; } = null!;

    public virtual Laboratory Laboratory { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;

    public virtual Reference SecondDoctor { get; set; } = null!;

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
    
}
