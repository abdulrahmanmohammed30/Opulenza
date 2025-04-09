using ErrorOr;
using MediatR;
using Opulenza.Application.Features.Common;

namespace Opulenza.Application.Features.Products.Queries.GetProducts;

public class GetProductsQuery: PaginatedQuery, IRequest<ErrorOr<GetProductListResult>>
{
    // free text search, scans the name, description fields
    public string? Search { get; set; }
    
    public string? Brand { get; init; }
    
    public string? Category{ get; set; }

    public int? MinRating { get; set; }

    public bool? IsAvailable { get; set; }
    
    public decimal? MinPrice { get; init; }
    
    public decimal? MaxPrice { get; init; }
    
    public bool? DiscountOnly { get; init; }
    
    public SortBy? SortBy { get; set; }
    
    public SortOptions? SortOptions { get; set; }
}