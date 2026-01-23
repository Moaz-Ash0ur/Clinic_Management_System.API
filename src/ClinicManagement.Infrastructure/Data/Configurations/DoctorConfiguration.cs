using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Doctors;
using ClinicManagement.Domain.DoctorWorkSchedules;
using ClinicManagement.Domain.Identity;
using ClinicManagement.Domain.Patients;
using ClinicManagement.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        // Table name
        builder.ToTable("Doctors");

        // Primary Key
        builder.HasKey(d => d.Id);

        // Properties
        builder.HasOne<AppUser>()
        .WithOne()
        .HasForeignKey<Doctor>(d => d.UserId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);


        builder.Property(d => d.Specialization)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(d => d.IsActive)
               .IsRequired();

        // Relationships

        // Doctor -> DoctorWorkSchedules (One-to-Many)
        builder.HasMany(d => d.WorkSchedules)
        .WithOne(ws => ws.Doctor)
        .HasForeignKey(ws => ws.DoctorId)
        .OnDelete(DeleteBehavior.Cascade);



        builder.HasIndex(d => d.UserId).IsUnique();

    }
}
