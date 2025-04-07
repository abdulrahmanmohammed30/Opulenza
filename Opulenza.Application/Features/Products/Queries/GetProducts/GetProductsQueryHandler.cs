using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;

namespace Opulenza.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductsQuery, ErrorOr<GetProductListResult>>
{
    public async Task<ErrorOr<GetProductListResult>> Handle(GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var results = await productRepository.GetProductsAsync(request, cancellationToken);
        return results;
    }
}