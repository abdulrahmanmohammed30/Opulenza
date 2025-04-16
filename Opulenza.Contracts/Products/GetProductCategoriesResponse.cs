namespace Opulenza.Contracts.Products;

public class GetProductCategoriesResponse
{
    public List<GetProductCategoryResponse> Categories { get; init; } = new();
}