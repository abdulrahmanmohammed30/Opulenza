using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Products;

namespace Opulenza.Infrastructure.Products.Persistence;

public class ProductMetadataConfigurations: IEntityTypeConfiguration<ProductMetadata>
{
    public void Configure(EntityTypeBuilder<ProductMetadata> builder)
    {
        builder.ToTable("ProductMetadata");
    
        builder.Property(p=>p.ProductId)
            .IsRequired();

        builder.Property(p => p.Key)
            .HasMaxLength(60)
            .IsRequired();
        
        builder.Property(p => p.Value)
            .HasMaxLength(300)
            .IsRequired();
    }
}
