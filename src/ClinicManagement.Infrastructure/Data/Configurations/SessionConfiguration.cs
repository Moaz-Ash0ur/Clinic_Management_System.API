using ClinicManagement.Domain.Patients.MedicalRecords;
using ClinicManagement.Domain.Prescriptions;
using ClinicManagement.Domain.Sessions;
using ClinicManagement.Domain.Sessions.Attendances;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        // Table name
        builder.ToTable("Sessions");

        // Primary Key
        builder.HasKey(s => s.Id);

        // Properties
        builder.Property(s => s.ActualStartTime)
               .IsRequired(false);

        builder.Property(s => s.ActualEndTime)
               .IsRequired(false);

        builder.Property(s => s.Status)
               .HasConversion<string>()
               .IsRequired();

        builder.Property(s => s.DoctorNotes)
               .HasMaxLength(1000)
               .IsRequired(false);

        // Relationships
      
        // Session -> Attendance (One-to-One required)
        builder.HasOne(s => s.attendance)
               .WithOne(a => a.Session)
               .HasForeignKey<Attendance>(a => a.SessionId)
               .OnDelete(DeleteBehavior.Cascade);

        // Session -> Prescription (One-to-One optional)
        builder.HasOne(s => s.Prescription)
               .WithOne(p => p.Session)
               .HasForeignKey<Prescription>(p => p.SessionId)
               .OnDelete(DeleteBehavior.Cascade);


        // Session -> MedicalRecords (One-to-Many)
        builder.HasMany(s => s.MedicalRecords)
        .WithOne(mr => mr.Session)
        .HasForeignKey(mr => mr.SessionId)
        .OnDelete(DeleteBehavior.Cascade);

    }







}

