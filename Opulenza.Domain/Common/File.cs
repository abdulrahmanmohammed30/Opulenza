namespace Opulenza.Domain.Common;

public abstract class File: BaseEntity
{
    public required string FilePath { get; set; }
    
    public string? FileName { get; set; }
    
    public string? OriginalFileName { get; set; }
}