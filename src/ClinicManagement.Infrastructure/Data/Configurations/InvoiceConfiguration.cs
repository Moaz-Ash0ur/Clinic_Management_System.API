using ClinicManagement.Domain.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.TotalAmount)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(i => i.Status)
               .IsRequired()
               .HasConversion<string>(); 

        builder.HasOne(i => i.Session)
               .WithOne()
               .HasForeignKey<Invoice>(i => i.SessionId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(i => i.Payments)
               .WithOne(p => p.Invoice)
               .HasForeignKey(p => p.InvoiceId)
               .OnDelete(DeleteBehavior.Restrict); 



    }
}

