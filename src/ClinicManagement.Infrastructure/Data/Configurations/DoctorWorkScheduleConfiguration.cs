using ClinicManagement.Domain.DoctorWorkSchedules;
using ClinicManagement.Domain.Prescriptions;
using ClinicManagement.Domain.Sessions;
using ClinicManagement.Domain.Sessions.Attendances;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

public class DoctorWorkScheduleConfiguration : IEntityTypeConfiguration<DoctorWorkSchedule>
{
    public void Configure(EntityTypeBuilder<DoctorWorkSchedule> builder)
    {
        // Table name
        builder.ToTable("DoctorWorkSchedules");

        // Primary Key
        builder.HasKey(ws => ws.Id);

        // Properties
        builder.Property(ws => ws.dayofWeek)
               .IsRequired();

        builder.Property(ws => ws.StartTime)
               .IsRequired()
               .HasColumnType("time");

        builder.Property(ws => ws.EndTime)
               .IsRequired()
               .HasColumnType("time");

          builder.HasIndex(ws => new { ws.DoctorId, ws.dayofWeek });

    }
}
