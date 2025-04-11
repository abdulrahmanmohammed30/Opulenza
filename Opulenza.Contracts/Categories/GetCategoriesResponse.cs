namespace Opulenza.Contracts.Common;

public class GetCategoriesResponse
{
    public List<GetCategoryResponse> Categories { get; init; } = new();
    public int TotalCount { get; init; }
}