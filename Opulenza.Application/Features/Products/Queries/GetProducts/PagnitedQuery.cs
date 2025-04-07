namespace Opulenza.Application.Features.Products.Queries.GetProducts;

public class PaginatedQuery
{
    public int PageNumber { get; set; } = 1;
    
    public int PageSize { get; set; } = 10;
}