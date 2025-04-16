using Opulenza.Application.Features.Common;
using Opulenza.Application.Features.Products.Common;

namespace Opulenza.Application.Features.Products.Commands.AddProductImages;

public class ProductImagesResult
{
    public int ProductId { get; init; }
    public required List<ImageResult> Images { get; init; }
    
    public List<string>? Warnings { get; init; }
}