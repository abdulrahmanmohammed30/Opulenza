using Opulenza.Application.Features.Common;
using Opulenza.Application.Features.Products.Common;

namespace Opulenza.Application.Features.Products.Queries.GetProductImages;

public class GetProductImagesResult
{
    public required List<ImageResult> Images { get; init; }

}