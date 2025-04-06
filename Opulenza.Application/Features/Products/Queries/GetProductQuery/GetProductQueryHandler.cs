using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Products.Queries.GetProductQuery;

public class GetProductQueryHandler: IRequestHandler<GetProductQuery, ErrorOr<ProductResult>>
{
    public async Task<ErrorOr<ProductResult>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        return new ProductResult();
    }
}