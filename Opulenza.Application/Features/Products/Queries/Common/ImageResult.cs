namespace Opulenza.Application.Features.Products.Queries.Common;

public class ImageResult
{
    public int Id { get; set; }
    public required string FilePath { get; set; }
    public bool IsFeaturedImage { get; set; }
}