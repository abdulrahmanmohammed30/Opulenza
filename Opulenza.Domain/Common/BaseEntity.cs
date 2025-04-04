namespace Opulenza.Domain.Common;

public abstract class BaseEntity: ISoftDeletable
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }
}
