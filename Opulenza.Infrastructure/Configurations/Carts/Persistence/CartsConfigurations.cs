using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Carts;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Infrastructure.Configurations.Carts.Persistence;

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
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}