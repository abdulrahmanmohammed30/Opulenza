using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Products;
using Opulenza.Domain.Entities.Ratings;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Infrastructure.Ratings.Persistence;

public class RatingConfigurations:IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        // builder.Property(p => p.Id)
        //     .ValueGeneratedOnAddOrUpdate()
        //     .IsRequired();
        //
        // builder
        //     .HasKey(u => u.Id);
        
        builder
            .Property(u => u.Value)
            .IsRequired();

        builder
            .Property(u => u.ReviewText)
            .HasColumnType("nvarchar(max)")
            .IsRequired(false);
        
        builder
            .Property(u => u.UserId)
            .IsRequired();

        builder
            .Property(u => u.ProductId)
            .IsRequired();

        builder.HasOne<Product>()
            .WithOne()
            .IsRequired()
            .HasForeignKey<Rating>(r=>r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<ApplicationUser>()
            .WithOne()
            .IsRequired()
            .HasForeignKey<Rating>(r=>r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}