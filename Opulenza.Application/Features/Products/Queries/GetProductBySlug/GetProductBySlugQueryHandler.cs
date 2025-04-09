using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Features.Products.Common;

namespace Opulenza.Application.Features.Products.Queries.GetProductBySlug;

public class GetProductBySlugQueryHandler(IProductRepository productRepository): IRequestHandler<GetProductBySlugQuery, ErrorOr<ProductResult>>
{
    public async Task<ErrorOr<ProductResult>> Handle(GetProductBySlugQuery request, CancellationToken cancellationToken)
    {
        var product= await productRepository.GetProductBySlugAsync(request.Slug,cancellationToken);
       
        if (product is null)
        {
            return Error.NotFound("ProductNotFound", "Product not found");
        }

        return product;
    }
}