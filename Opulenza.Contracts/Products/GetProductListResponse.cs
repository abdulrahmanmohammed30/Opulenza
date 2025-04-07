namespace Opulenza.Contracts.Products;

public class GetProductListResponse
{
    public required List<GetProductListItemResponse> Products { get; init; }
    public required int TotalCount { get; set; }
}