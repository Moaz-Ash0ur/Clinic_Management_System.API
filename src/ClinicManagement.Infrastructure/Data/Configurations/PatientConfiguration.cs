using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Identity;
using ClinicManagement.Domain.Patients;
using ClinicManagement.Domain.Patients.MedicalRecords;
using ClinicManagement.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        // Table name
        builder.ToTable("Patients");

        // Primary Key
        builder.HasKey(p => p.Id);

        // Properties
        builder.HasOne(p => p.User)
        .WithOne()         
        .HasForeignKey<Patient>(p => p.UserId) 
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);


        builder.Property(p => p.DateOfBirth)
               .IsRequired();

        builder.Property(p => p.Gender)
               .HasConversion<string>()
               .IsRequired();

        builder.Property(p => p.IsActive)
               .IsRequired();

        // Relationships

        // Patient -> MedicalRecords (One-to-Many)
        builder.HasMany(p => p.MedicalRecords)
        .WithOne(mr => mr.Patient)
        .HasForeignKey(mr => mr.PatientId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Attendances)
       .WithOne(a => a.Patient)
       .HasForeignKey(a => a.PatientId)
       .OnDelete(DeleteBehavior.Restrict);


        builder.HasIndex(p => p.UserId).IsUnique();

    }

}
