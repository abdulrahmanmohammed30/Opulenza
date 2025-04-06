using ErrorOr;
using MediatR;
using Opulenza.Application.Features.Products.Queries.Common;

namespace Opulenza.Application.Features.Products.Queries.GetProductId;

public class GetProductByIdQuery: IRequest<ErrorOr<ProductResult>>
{
    public int Id { get; init; }
}
