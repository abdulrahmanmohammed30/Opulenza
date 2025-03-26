using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Orders;
using Opulenza.Domain.Entities.Products;

namespace Opulenza.Infrastructure.Configurations.Orders.Persistence;

public class OrderItemConfigurations: IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        // builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(oi => oi.TotalPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(oi => oi.Tax)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.Property(oi => oi.TaxIncluded)
            .IsRequired();
        
        builder.HasOne<Order>()
               .WithMany(o => o.Items)
               .HasForeignKey(o => o.OrderId)
               .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<Product>()
               .WithMany()
               .HasForeignKey(p => p.ProductId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}