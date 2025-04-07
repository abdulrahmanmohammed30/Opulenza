namespace Opulenza.Application.Features.Products.Queries.GetProducts;

public class GetProductListResult
{
    public required List<GetProductListItemResult> Products { get; init; }
    public required int TotalCount { get; set; }
}