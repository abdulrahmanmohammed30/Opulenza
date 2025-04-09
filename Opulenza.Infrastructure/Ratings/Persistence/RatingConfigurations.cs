using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Products;
using Opulenza.Domain.Entities.Ratings;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Infrastructure.Ratings.Persistence;

public class RatingConfigurations : IEntityTypeConfiguration<Rating>
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

        builder.Property(u => u.ProductId)
            .IsRequired();

        builder.HasOne(r => r.User)
            .WithMany(user=>user.Ratings)
            .HasForeignKey(r => r.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Product)
            .WithMany(p=>p.Ratings)
            .HasForeignKey(r => r.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasIndex(r => new { r.UserId, r.ProductId })
            .IsUnique()
            .HasDatabaseName("IX_User_Product");
    }
}