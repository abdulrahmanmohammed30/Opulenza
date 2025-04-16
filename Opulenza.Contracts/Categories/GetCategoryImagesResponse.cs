using Opulenza.Contracts.Common;

namespace Opulenza.Contracts.Categories;

public class GetCategoryImagesResponse
{
    public List<ImageResponse> Images { get; set; } = new();
}
