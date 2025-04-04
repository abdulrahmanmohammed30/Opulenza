using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Infrastructure.Users.Persistence;

public class UserAddressConfigurations : IEntityTypeConfiguration<UserAddress>
{
    public void Configure(EntityTypeBuilder<UserAddress> builder)
    {
        builder.ToTable("UserAddresses");
            
        builder
            .Property(u => u.StreetAddress)
            .HasMaxLength(255);

        builder
            .Property(u => u.Country)
            .HasMaxLength(100);

        builder
            .Property(u => u.City)
            .HasMaxLength(100);

        builder
            .Property(u => u.ZipCode)
            .HasMaxLength(20);
    }
}