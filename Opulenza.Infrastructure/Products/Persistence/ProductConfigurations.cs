using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Products;

namespace Opulenza.Infrastructure.Products.Persistence;

public class ProductConfigurations: IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name)
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .IsRequired();
       
        builder.Property(p => p.Description)
            .HasColumnType("varchar(max)")
            .IsRequired();            

        // [Name]_[Id] 
        // Name -> 50
        //  2,147,483,647 -> 10 characters after conversion 
        builder.Property(p=>p.Slug)
            .HasColumnType("varchar")
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(p => p.Price)
            .HasColumnType("DECIMAL(18,2)")
            .IsRequired();
        
        builder.Property(p=>p.DiscountPrice)
            .HasColumnType("DECIMAL(18,2)")
            .IsRequired(false);
        
        builder.Property(p=>p.Tax)
            .HasColumnType("DECIMAL(18,2)")
            .IsRequired();

        builder.Property(p => p.Brand)
            .HasColumnType("varchar")
            .HasMaxLength(80)
            .IsRequired(false);

        builder.Property(p => p.StockQuantity)
            .IsRequired(false);

        // Relation is many-to-many 
        // A product can have many images 
        // If a product was deleted, all images related to that product will be deleted
        builder
            .HasMany<ProductImage>(p => p.Images)
            .WithOne()
            .HasForeignKey(p=>p.ProductId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder.HasMany(p => p.Categories)
            .WithMany(c => c.Products);
        
    }
}