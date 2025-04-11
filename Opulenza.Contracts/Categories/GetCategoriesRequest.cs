using Opulenza.Contracts.Common;

namespace Opulenza.Contracts.Categories;

public class GetCategoriesRequest: PaginatedRequest
{
    public bool? Sort { get; set; }
    public string? Search { get; set; }
}

