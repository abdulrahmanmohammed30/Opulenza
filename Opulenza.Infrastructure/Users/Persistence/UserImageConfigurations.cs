using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Infrastructure.Users.Persistence;

public class UserImageConfigurations: IEntityTypeConfiguration<UserImage>
{
    public void Configure(EntityTypeBuilder<UserImage> builder)
    {
        builder.ToTable("UserImages");
    }
}