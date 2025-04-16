using Opulenza.Application.Features.Common;

namespace Opulenza.Application.Features.Categories.Queries.GetCategoryImages;

public class GetCategoryImagesResult
{
    public required List<ImageResult> Images { get; init; }

}