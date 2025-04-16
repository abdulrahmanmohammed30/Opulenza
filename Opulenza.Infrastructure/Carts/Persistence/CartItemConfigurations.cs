using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Carts;
using Opulenza.Domain.Entities.Products;

namespace Opulenza.Infrastructure.Carts.Persistence;

public class CartItemConfigurations: IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        //builder.HasKey(ci => ci.Id);
        builder.ToTable("CartItems");
        
        builder.Property(ci => ci.CartId)
            .IsRequired();

        builder.Property(ci => ci.ProductId)
            .IsRequired();

        builder.Property(ci => ci.Quantity)
            .IsRequired();

        builder.HasOne<Cart>()
            .WithMany()
            .HasForeignKey(ci => ci.CartId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<Product>(x=>x.Product)
            .WithMany()
            .HasForeignKey(ci => ci.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
