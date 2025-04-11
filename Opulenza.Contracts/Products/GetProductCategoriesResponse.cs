namespace Opulenza.Contracts.Products;

public class GetCategoriesResponse
{
    public List<GetProductCategoryResponse> Categories { get; init; } = new();
}