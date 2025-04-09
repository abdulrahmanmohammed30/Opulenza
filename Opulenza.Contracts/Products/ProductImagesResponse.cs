namespace Opulenza.Contracts.Products;

public class ProductImagesResponse
{
    public int ProductId { get; init; }
    public required List<ImageResponse> Images { get; init; }
    
    public List<string>? Warnings { get; init; }
}