using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = Opulenza.Domain.Common.File;

namespace Opulenza.Infrastructure.Common.Persistence;

public class FileConfigurations : IEntityTypeConfiguration<File>
{
    public void Configure(EntityTypeBuilder<File> builder)
    {
        // builder.Property(p => p.Id)
        //     .ValueGeneratedOnAddOrUpdate()
        //     .IsRequired();

        // builder
        //     .HasKey(u => u.Id);

        builder
            .Property(u => u.FilePath)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(u => u.FileName)
            .HasMaxLength(255);

        builder
            .Property(u => u.OriginalFileName)
            .HasMaxLength(255);

        builder.UseTpcMappingStrategy();
    }
}
