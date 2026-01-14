using ClinicManagement.Domain.Patients.MedicalRecords;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

public class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
{
    public void Configure(EntityTypeBuilder<MedicalRecord> builder)
    {
        // Table name
        builder.ToTable("MedicalRecords");

        // Primary Key
        builder.HasKey(mr => mr.Id);

        // Properties
        builder.Property(mr => mr.Diagnosis)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(mr => mr.Notes)
               .HasMaxLength(500)
               .IsRequired(false);

        // 

    }

}
