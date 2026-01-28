using ClinicManagement.Domain.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

public sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(i => i.IsPaid)
            .IsRequired();

        builder.HasOne(i => i.Session)
            .WithOne()
            .HasForeignKey<Invoice>(i => i.SessionId)
            .OnDelete(DeleteBehavior.Restrict);

 
    }
}

