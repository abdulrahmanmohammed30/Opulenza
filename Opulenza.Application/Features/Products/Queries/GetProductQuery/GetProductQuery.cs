using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Products.Queries.GetProductQuery;

public class GetProductQuery: IRequest<ErrorOr<ProductResult>>
{
    public int Id { get; init; }
}
