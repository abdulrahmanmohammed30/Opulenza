using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Enums;
using Opulenza.Domain.Entities.Orders;
using Opulenza.Domain.Entities.Payments;

namespace Opulenza.Infrastructure.Payments.Persistence;

public class PaymentConfigurations:IEntityTypeConfiguration<Payment> 
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        // builder.Property(p => p.Id).ValueGeneratedOnAdd();
        //
        // builder.HasKey(p => p.Id);

        builder.Property(p => p.PaymentStatus)
            .HasColumnType("varchar")
            .HasMaxLength(20)
            .HasConversion(
                p => p.ToString(),
                p => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), p));
        
        builder.Property(p => p.PaymentMethod)
            .HasColumnType("varchar")
            .HasMaxLength(20)
            .HasConversion(
                p => p.ToString(),
                p => (PaymentMethod)Enum.Parse(typeof(PaymentMethod), p));

        builder.Property(p => p.OrderId)
            .IsRequired();
    }
}
