namespace Opulenza.Domain;

public class File
{
    public int Id  { get; set; }

    public required string FilePath { get; set; }
    
    public string? FileName { get; set; }
    
    public string? OriginalFileName { get; set; }
}