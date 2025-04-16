using Opulenza.Contracts.Common;

namespace Opulenza.Contracts.Categories;

public class CategoryImagesResponse
{
    public int CategoryId { get; init; }
    public required List<ImageResponse> Images { get; init; }
    
    public List<string>? Warnings { get; init; }
}