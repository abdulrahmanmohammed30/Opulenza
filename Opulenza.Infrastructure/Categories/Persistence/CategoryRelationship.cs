using Microsoft.EntityFrameworkCore;

namespace Opulenza.Infrastructure.Categories.Persistence;

[Keyless]
public class CategoryRelationship
{
    public int Id { get; set; }
    public int ParentId { get; set; }
}