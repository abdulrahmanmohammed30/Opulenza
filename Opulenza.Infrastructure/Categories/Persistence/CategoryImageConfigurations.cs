using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opulenza.Domain.Entities.Categories;

namespace Opulenza.Infrastructure.Categories.Persistence;

public class CategoryImageConfigurations: IEntityTypeConfiguration<CategoryImage>
{
    public void Configure(EntityTypeBuilder<CategoryImage> builder)
    {
        builder.ToTable("CategoryImages");
    }
}