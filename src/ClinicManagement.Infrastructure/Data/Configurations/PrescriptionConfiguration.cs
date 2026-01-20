using ClinicManagement.Domain.Prescriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
{
    public void Configure(EntityTypeBuilder<Prescription> builder)
    {
        // Table name
        builder.ToTable("Prescriptions");

        // Primary Key
        builder.HasKey(p => p.Id);

        // Properties
        builder.Property(p => p.MedicationName)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(p => p.Dosage)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(p => p.Description)
               .IsRequired()
               .HasMaxLength(500);
               



    }
}
