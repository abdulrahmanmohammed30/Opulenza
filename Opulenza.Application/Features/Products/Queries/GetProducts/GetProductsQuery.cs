namespace Opulenza.Application.Features.Products.Queries.GetProducts;

public class GetProductsQuery
{
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool? DiscountOnly { get; set; }
}