namespace Opulenza.Domain.Common;

public abstract class BaseEntity:Entity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }
}
