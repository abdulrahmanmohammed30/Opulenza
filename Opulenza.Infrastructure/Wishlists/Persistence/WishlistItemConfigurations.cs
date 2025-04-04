using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Users;
using Opulenza.Domain.Entities.Wishlists;

namespace Opulenza.Infrastructure.Wishlists.Persistence;

public class WishlistItemConfigurations:IEntityTypeConfiguration<WishListItem>
{
    public void Configure(EntityTypeBuilder<WishListItem> builder)
    {
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
            .WithMany(u=>u.WishListItems)
            .HasForeignKey(w=>w.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}

