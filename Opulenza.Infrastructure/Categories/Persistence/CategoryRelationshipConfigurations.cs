using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Opulenza.Infrastructure.Categories.Persistence;

public class CategoryRelationshipConfigurations: IEntityTypeConfiguration<CategoryRelationship>
{
    public void Configure(EntityTypeBuilder<CategoryRelationship> builder)
    {
        builder.HasNoKey();
    }
}