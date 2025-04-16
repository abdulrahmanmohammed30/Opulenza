using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Sessions;

namespace Opulenza.Infrastructure.Sessions;

public class SessionConfigurations: IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.Property(p=>p.SessionId)
            .HasMaxLength(300)
            .IsRequired();
    }
}