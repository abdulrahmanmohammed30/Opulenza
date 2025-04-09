namespace Opulenza.Application.Features.Common;

public class PaginatedQuery
{
    public int PageNumber { get; set; } = 1;
    
    public int PageSize { get; set; } = 10;
}