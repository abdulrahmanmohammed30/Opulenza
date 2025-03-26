using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Users;
using Opulenza.Domain.Entities.Wishlists;

namespace Opulenza.Infrastructure.Wishlists.Persistence;

public class WishlistConfigurations:IEntityTypeConfiguration<Wishlist>
{
    public void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        // builder.Property(p => p.Id)
        //     .ValueGeneratedOnAddOrUpdate()
        //     .IsRequired();
        //
        // builder
        //     .HasKey(u => u.Id);
        //
        builder
            .Property(u => u.UserId)
            .IsRequired();

        builder
            .Property(u => u.ProductId)
            .IsRequired();

        builder.HasOne(p=>p.Product)
            .WithOne()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<Wishlist>(w=>w.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}

