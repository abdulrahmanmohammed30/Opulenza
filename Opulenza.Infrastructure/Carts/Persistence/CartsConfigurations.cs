using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Carts;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Infrastructure.Carts.Persistence;

public class CartsConfigurations: IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        //builder.HasKey(c => c.Id);

        builder.Property(c => c.UserId)
            .IsRequired();

        builder.Property(c => c.TotalPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(c => c.TotalPriceAfterDiscount)
            .IsRequired(false)
            .HasColumnType("decimal(18,2)");

        builder.HasMany(c => c.Items)
            .WithOne() 
            .HasForeignKey(ci => ci.CartId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<ApplicationUser>()
            .WithOne(u=>u.Cart)
            .HasForeignKey<Cart>(c=> c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}