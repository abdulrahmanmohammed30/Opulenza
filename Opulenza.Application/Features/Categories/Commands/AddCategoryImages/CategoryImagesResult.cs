using Opulenza.Application.Features.Common;
using Opulenza.Application.Features.Products.Common;

namespace Opulenza.Application.Features.Categories.Commands.AddCategoryImages;

public class CategoryImagesResult
{
    public int CategoryId { get; init; }
    public required List<ImageResult> Images { get; init; }
    
    public List<string>? Warnings { get; init; }
}