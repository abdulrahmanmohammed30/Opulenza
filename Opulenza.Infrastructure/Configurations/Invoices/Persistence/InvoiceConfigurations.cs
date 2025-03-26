using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Invoices;
using Opulenza.Domain.Entities.Orders;

namespace Opulenza.Infrastructure.Configurations.Invoices.Persistence;

public class InvoiceConfigurations: IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {       
        // builder.Property(p => p.Id)
        //     .ValueGeneratedOnAddOrUpdate()
        //     .IsRequired();
        //
        // builder.HasKey(p => p.Id);

        builder.Property(p => p.InvoiceUrl)
            .HasMaxLength(2048)
            .IsRequired();

        builder.Property(p => p.OrderId)
            .IsRequired();

        builder.HasOne<Order>()
            .WithOne()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}