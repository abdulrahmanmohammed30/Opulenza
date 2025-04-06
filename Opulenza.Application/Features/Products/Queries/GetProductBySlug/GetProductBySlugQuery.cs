using ErrorOr;
using MediatR;
using Opulenza.Application.Features.Products.Queries.Common;

namespace Opulenza.Application.Features.Products.Queries.GetProductBySlug;

public class GetProductBySlugQuery: IRequest<ErrorOr<ProductResult>>
{
    public string? Slug { get; init; }
}