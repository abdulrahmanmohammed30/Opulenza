using ErrorOr;
using MediatR;
using Opulenza.Application.Features.Common;

namespace Opulenza.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQuery: PaginatedQuery, IRequest<ErrorOr<GetCategoriesResult>>
{
    public bool? Sort { get; set; }
    public string? Search { get; set; }
}