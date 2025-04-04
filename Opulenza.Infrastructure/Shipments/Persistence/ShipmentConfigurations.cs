using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Shipments;

namespace Opulenza.Infrastructure.Shipments.Persistence;

public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        // builder.Property(s=>s.Id)
        //     .ValueGeneratedOnAddOrUpdate()
        //     .IsRequired();
        //
        // builder.HasKey(s => s.Id);

        builder.Property(p => p.OrderId)
            .IsRequired();
        
        builder.HasOne(s => s.Order)
            .WithMany()
            .HasForeignKey(s => s.OrderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Configure the relationship with UserAddress.
        // Although the navigation property is nullable at the application level,
        // the foreign key (UserAddressId) is required in the database
        
        builder.HasOne(s => s.UserAddress)
            .WithMany()
            .HasForeignKey(s => s.UserAddressId)
            .OnDelete(DeleteBehavior.SetNull);

        // Mark required string properties.
        builder
            .Property(s => s.ShippingCompany)
            .HasMaxLength(60)
            .IsRequired();
        
        builder
            .Property(s => s.ShippingTrackId)
            .HasMaxLength(50)
            .IsRequired();
            
        builder
            .Property(s => s.ShippingTracKUrl)
            .HasColumnType("nvarchar")
            .HasMaxLength(2048)
            .IsRequired();
    }
}