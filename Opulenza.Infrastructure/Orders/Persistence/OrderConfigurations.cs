using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Enums;
using Opulenza.Domain.Entities.Orders;
using Opulenza.Domain.Entities.Payments;
using Opulenza.Domain.Entities.Shipments;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Infrastructure.Orders.Persistence;

public class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(o => o.OrderStatus)
            .HasConversion(
                p => p.ToString(),
                p => (OrderStatus)Enum.Parse(typeof(OrderStatus), p)
            )
            .IsRequired();
        
        builder.Property(o => o.PaymentId)
            .IsRequired();

        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey(i => i.OrderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Payment>()
            .WithOne()
            .HasForeignKey<Payment>(p => p.OrderId)
            .IsRequired();
        
        builder.HasOne<Shipment>()
            .WithOne(s => s.Order)
            .HasForeignKey<Shipment>(s=>s.OrderId)
            .IsRequired(false);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}