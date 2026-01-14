using ClinicManagement.Domain.Patients;
using ClinicManagement.Domain.Prescriptions;
using ClinicManagement.Domain.Sessions;
using ClinicManagement.Domain.Sessions.Attendances;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        // Table name
        builder.ToTable("Attendances");

        // Primary Key
        builder.HasKey(a => a.Id);

        // Properties
        builder.Property(a => a.Date)
               .IsRequired();

        builder.Property(a => a.Status)
               .HasConversion<string>() 
               .IsRequired();
    }


}
