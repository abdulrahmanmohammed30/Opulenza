using Opulenza.Contracts.Common;

namespace Opulenza.Contracts.Products;

public class GetProductsRequest: PaginatedRequest
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
    
    public string? Sort { get; set; }
}