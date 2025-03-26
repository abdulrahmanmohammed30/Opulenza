using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Infrastructure.Products.Users.Persistence;

public class ApplicationUserConfigurations:IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(p => p.FirstName)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(p => p.LastName)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.HasOne<UserAddress>()
            .WithOne()
            .HasForeignKey<UserAddress>(a => a.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<UserImage>()
            .WithOne()      
            .HasForeignKey<UserImage>(a => a.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}