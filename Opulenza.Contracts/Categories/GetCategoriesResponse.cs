namespace Opulenza.Contracts.Categories;

public class GetCategoriesResponse
{
    public List<GetCategoryResponse> Categories { get; init; } = new();
    public int TotalCount { get; init; }
}