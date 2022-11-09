using Microsoft.EntityFrameworkCore;
using System;

namespace Tutorial8.Models
{
    public class MedsDbContext : DbContext
    {
        public MedsDbContext()
        { }

        public MedsDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Prescription_Medicament> Prescription_Medicaments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s20218;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Polish_CI_AS");

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(d => d.IdDoctor)
                    .HasName("Doctor_pk");

                entity.ToTable("Doctor");

                entity.Property(d => d.IdDoctor).ValueGeneratedOnAdd();

                entity.Property(d => d.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(d => d.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(d => d.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasData(
                        new Doctor {IdDoctor = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jkowal@gmail.com"},
                        new Doctor {IdDoctor = 2, FirstName = "John", LastName = "Brown", Email = "jbrown@gmail.com" }
                    );

            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.IdPatient)
                    .HasName("Patient_pk");

                entity.ToTable("Patient");

                entity.Property(p => p.IdPatient).ValueGeneratedOnAdd();

                entity.Property(p => p.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(p => p.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(p => p.Birthdate).HasColumnType("datetime");

                entity.HasData(
                       new Patient { IdPatient = 1, FirstName = "Ann", LastName = "Kowalska", Birthdate = new DateTime(1997, 12, 25)},
                       new Patient { IdPatient = 2, FirstName = "Tom", LastName = "Reddle", Birthdate = new DateTime(1995, 7, 3) }
                   );
            });

            modelBuilder.Entity<Medicament>(entity =>
            {
                entity.HasKey(m => m.IdMedicament)
                    .HasName("Medicament_pk");

                entity.ToTable("Medicament");

                entity.Property(m => m.IdMedicament).ValueGeneratedOnAdd();

                entity.Property(m => m.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(m => m.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(m => m.Type)
                   .IsRequired()
                   .HasMaxLength(100);

                entity.HasData(
                       new Medicament { IdMedicament = 1, Name = "Sominex", Description = "Difficulty sleeping", Type = "Psychotropic" },
                       new Medicament { IdMedicament = 2, Name = "Norpramin", Description = "Depression", Type = "Psychotropic" }
                   );
            });

            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasKey(pr => pr.IdPrescription)
                    .HasName("Prescription_pk");

                entity.ToTable("Prescription");

                entity.Property(pr => pr.IdPrescription).ValueGeneratedOnAdd();

                entity.Property(pr => pr.Date).HasColumnType("datetime");

                entity.Property(pr => pr.DueDate).HasColumnType("datetime");

                entity.HasOne(pr => pr.IdPatientNavigation)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(pr => pr.IdPatient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Prescription_Patient");

                entity.HasOne(pr => pr.IdDoctorNavigation)
                    .WithMany(d => d.Prescriptions)
                    .HasForeignKey(pr => pr.IdDoctor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Prescription_Doctor");

                entity.HasData(
                       new Prescription { IdPrescription = 1, Date = new DateTime(2021, 2, 3), DueDate = new DateTime(2021, 9, 9), IdDoctor = 1, IdPatient = 1 },
                       new Prescription { IdPrescription = 2, Date = new DateTime(2021, 5, 5), DueDate = new DateTime(2022, 7, 1), IdDoctor = 1, IdPatient = 2 }
                   );

            });

            modelBuilder.Entity<Prescription_Medicament>(entity =>
            {
                entity.HasKey(pm => new { pm.IdMedicament, pm.IdPrescription })
                   .HasName("Prescription_Medicament_pk");

                entity.ToTable("Prescription_Medicament");

                entity.HasOne(pm => pm.IdMedicamentNavigation)
                    .WithMany(m => m.Prescription_Medicaments)
                    .HasForeignKey(pm => pm.IdMedicament)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Prescription_Medicament_Medicament");

                entity.HasOne(pm => pm.IdPrescriptionNavigation)
                    .WithMany(p => p.Prescription_Medicaments)
                    .HasForeignKey(pm => pm.IdPrescription)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Prescription_Medicament_Prescription");

                entity.Property(pm => pm.Details)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasData(
                       new Prescription_Medicament { IdMedicament = 1, IdPrescription = 1, Details = "no information", Dose = null },
                       new Prescription_Medicament { IdMedicament = 2, IdPrescription = 2, Details = "no information", Dose = null }
                   );
            });
        }
    }
}
