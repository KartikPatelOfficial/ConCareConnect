using DiagnosticServicesModel.DataClass;
using DiagnosticServicesModel.Request;
using Microsoft.EntityFrameworkCore;

namespace PathologyBackend.Models;

public partial class DiagnosticServiceContext : DbContext
{
    public DiagnosticServiceContext()
    {
    }

    public DiagnosticServiceContext(DbContextOptions<DiagnosticServiceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Laboratory> Laboratories { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Reference> References { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=DiagnosticService;Username=postgres;Password=Edgeis10");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<ParameterRequest>();
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("appointment_pkey");

            entity.ToTable("appointment");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('appointment_pd_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnName("createdat");
            entity.Property(e => e.Doctorid).HasColumnName("doctorid");
            entity.Property(e => e.Doctormargin).HasColumnName("doctormargin");
            entity.Property(e => e.Expenses).HasColumnName("expenses");
            entity.Property(e => e.Labcharges).HasColumnName("labcharges");
            entity.Property(e => e.Laboratoryid)
                .HasColumnType("character varying")
                .HasColumnName("laboratoryid");
            entity.Property(e => e.Patientid).HasColumnName("patientid");
            entity.Property(e => e.Patienttype)
                .HasMaxLength(255)
                .HasColumnName("patienttype");
            entity.Property(e => e.Paymentstatus)
                .HasMaxLength(255)
                .HasDefaultValueSql("'UNPAID'::character varying")
                .HasColumnName("paymentstatus");
            entity.Property(e => e.Reportedat).HasColumnName("reportedat");
            entity.Property(e => e.Sampletype)
                .HasMaxLength(255)
                .HasColumnName("sampletype");
            entity.Property(e => e.SecondDoctorid).HasColumnName("second_doctorid");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasDefaultValueSql("'CREATED'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Total).HasColumnName("total");

            entity.HasOne(d => d.Doctor).WithMany(p => p.AppointmentDoctors)
                .HasForeignKey(d => d.Doctorid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_doctorid_fkey");

            entity.HasOne(d => d.Laboratory).WithMany(p => p.Appointments)
                .HasPrincipalKey(p => p.Firebaseuid)
                .HasForeignKey(d => d.Laboratoryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_laboratory_firebaseuid_fk");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.Patientid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_patientid_fkey");

            entity.HasOne(d => d.SecondDoctor).WithMany(p => p.AppointmentSecondDoctors)
                .HasForeignKey(d => d.SecondDoctorid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_second_doctorid_fkey");
        });

        modelBuilder.Entity<Laboratory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("laboratory_pkey");

            entity.ToTable("laboratory");

            entity.HasIndex(e => e.Firebaseuid, "laboratory_pk").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Addressline1)
                .HasMaxLength(255)
                .HasColumnName("addressline1");
            entity.Property(e => e.Addressline2)
                .HasMaxLength(255)
                .HasColumnName("addressline2");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnName("createdat");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Firebaseuid)
                .HasDefaultValueSql("''::character varying")
                .HasColumnType("character varying")
                .HasColumnName("firebaseuid");
            entity.Property(e => e.Logo)
                .HasMaxLength(255)
                .HasColumnName("logo");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .HasColumnName("phone");
            entity.Property(e => e.Pincode)
                .HasMaxLength(255)
                .HasColumnName("pincode");
            entity.Property(e => e.Sign)
                .HasMaxLength(255)
                .HasColumnName("sign");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("patient_pkey");

            entity.ToTable("patient");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnName("createdat");
            entity.Property(e => e.Dob)
                .HasMaxLength(255)
                .HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(255)
                .HasColumnName("gender");
            entity.Property(e => e.Laboratoryid)
                .HasColumnType("character varying")
                .HasColumnName("laboratoryid");
            entity.Property(e => e.Medicalhistory).HasColumnName("medicalhistory");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .HasColumnName("phone");
            entity.Property(e => e.Remarks).HasColumnName("remarks");

            entity.HasOne(d => d.Laboratory).WithMany(p => p.Patients)
                .HasPrincipalKey(p => p.Firebaseuid)
                .HasForeignKey(d => d.Laboratoryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("patient_laboratory_firebaseuid_fk");
        });

        modelBuilder.Entity<Reference>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reference_pkey");

            entity.ToTable("reference");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnName("createdat");
            entity.Property(e => e.Cutoff).HasColumnName("cutoff");
            entity.Property(e => e.Hospital)
                .HasMaxLength(255)
                .HasColumnName("hospital");
            entity.Property(e => e.Laboratoryid)
                .HasColumnType("character varying")
                .HasColumnName("laboratoryid");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .HasColumnName("phone");
            entity.Property(e => e.Qualification)
                .HasMaxLength(255)
                .HasColumnName("qualification");

            entity.HasOne(d => d.Laboratory).WithMany(p => p.References)
                .HasPrincipalKey(p => p.Firebaseuid)
                .HasForeignKey(d => d.Laboratoryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reference_laboratory_firebaseuid_fk");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("test_pkey");

            entity.ToTable("test");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Appointmentid).HasColumnName("appointmentid");
            entity.Property(e => e.Categorynote)
                .HasMaxLength(255)
                .HasColumnName("categorynote");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnName("createdat");
            entity.Property(e => e.Expenses).HasColumnName("expenses");
            entity.Property(e => e.Laboratoryid)
                .HasColumnType("character varying")
                .HasColumnName("laboratoryid");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Result)
                .HasColumnType("jsonb[]")
                .HasColumnName("result");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NEW'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Testid).HasColumnName("testid");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Tests)
                .HasForeignKey(d => d.Appointmentid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("test_appointmentid_fkey");

            entity.HasOne(d => d.Laboratory).WithMany(p => p.Tests)
                .HasPrincipalKey(p => p.Firebaseuid)
                .HasForeignKey(d => d.Laboratoryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("test_laboratory_firebaseuid_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
