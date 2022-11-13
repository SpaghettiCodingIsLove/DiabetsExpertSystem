using Microsoft.EntityFrameworkCore;

namespace DiabetsAPI.DB;

public partial class DiabetsContext : DbContext
{
    public DiabetsContext()
    {
    }

    public DiabetsContext(DbContextOptions<DiabetsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Examination> Examinations { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.ToTable("DOCTOR");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IsAdmin).HasColumnName("IS_ADMIN");
            entity.Property(e => e.LastName).HasColumnName("LAST_NAME");
            entity.Property(e => e.Login)
                .IsRequired()
                .HasColumnName("LOGIN");
            entity.Property(e => e.Name).HasColumnName("NAME");
            entity.Property(e => e.Password)
                .IsRequired()
                .HasColumnName("PASSWORD");
        });

        modelBuilder.Entity<Examination>(entity =>
        {
            entity.ToTable("EXAMINATION");

            entity.HasIndex(e => e.DoctorId, "IX_EXAMINATION_DOCTOR_ID");

            entity.HasIndex(e => e.PatientId, "IX_EXAMINATION_PATIENT_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Date)
                .IsRequired()
                .HasColumnName("DATE");
            entity.Property(e => e.DoctorId).HasColumnName("DOCTOR_ID");
            entity.Property(e => e.Measures)
                .IsRequired()
                .HasColumnName("MEASURES");
            entity.Property(e => e.PatientId).HasColumnName("PATIENT_ID");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Examinations)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EXAMINATION_DOCTOR");

            entity.HasOne(d => d.Patient).WithMany(p => p.Examinations)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EXAMINATION_PATIENT");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.ToTable("PATIENT");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BirthDate).HasColumnName("BIRTH_DATE");
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasColumnName("LAST_NAME");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("NAME");
            entity.Property(e => e.Pesel)
                .IsRequired()
                .HasColumnName("PESEL");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
