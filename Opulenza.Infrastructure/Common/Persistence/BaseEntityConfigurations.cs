﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Common;

namespace Opulenza.Infrastructure.Common.Persistence;

public class BaseEntityConfigurations: IEntityTypeConfiguration<BaseEntity>
{
    public void Configure(EntityTypeBuilder<BaseEntity> builder)
    {
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();
        
        builder.Property(x => x.UpdatedAt)
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        
        builder.UseTpcMappingStrategy();

        builder.HasQueryFilter(x => x.IsDeleted == false);
    }
}