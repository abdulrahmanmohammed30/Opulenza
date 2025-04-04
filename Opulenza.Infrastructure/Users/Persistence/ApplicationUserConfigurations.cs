using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Infrastructure.Users.Persistence;

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
        
        builder.HasOne(u=>u.Address)
            .WithOne()
            .HasForeignKey<UserAddress>(a => a.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(u=>u.Image)
            .WithOne()      
            .HasForeignKey<UserImage>(a => a.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.RefreshToken)
            .HasMaxLength(88)
            .IsRequired(false);
        
        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("getdate()");
        
        builder.Property(p => p.UpdatedAt)
            .HasDefaultValueSql("getdate()");
    }
}