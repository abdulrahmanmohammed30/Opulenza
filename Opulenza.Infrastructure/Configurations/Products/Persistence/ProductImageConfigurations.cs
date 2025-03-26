using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Products;

namespace Opulenza.Infrastructure.Products.Persistence;

public class ProductImageConfigurations: IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("ProductImages");
        
        builder
            .Property(p => p.ProductId)
            .IsRequired();
    }
}