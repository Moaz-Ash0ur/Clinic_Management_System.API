using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Doctors;
using ClinicManagement.Domain.DoctorWorkSchedules;
using ClinicManagement.Domain.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.ScheduledAt)
               .IsRequired();

        builder.Property(a => a.Duration)
               .IsRequired();

        builder.Property(a => a.Status)
               .HasConversion<string>()
               .IsRequired();

        // Relationships 

        // Appointment -> Doctor (Many-to-One)
        builder.HasOne(a => a.Doctor)
               .WithMany(d => d.Appointments)
               .HasForeignKey(a => a.DoctorId)
               .OnDelete(DeleteBehavior.Restrict);

        // Appointment -> Patient (Many-to-One)
        builder.HasOne(a => a.Patient)
               .WithMany(p => p.Appointments) 
               .HasForeignKey(a => a.PatientId)
               .OnDelete(DeleteBehavior.Restrict);

        // Appointment -> Session (One-to-One optional)
        builder.HasOne(a => a.Session)
               .WithOne(s => s.appointment) 
               .HasForeignKey<Session>(s => s.AppointmentId)
               .OnDelete(DeleteBehavior.Cascade);


        builder.HasIndex(a => a.DoctorId);
        builder.HasIndex(a => a.PatientId);
        builder.HasIndex(a => new { a.DoctorId, a.ScheduledAt });

      




    }

}




