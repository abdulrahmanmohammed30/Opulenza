using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Features.Products.Queries.Common;

namespace Opulenza.Application.Features.Products.Queries.GetProductId;

public class GetProductByIdQueryHandler(IProductRepository productRepository): IRequestHandler<GetProductByIdQuery, ErrorOr<ProductResult>>
{
    public async Task<ErrorOr<ProductResult>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
       var product= await productRepository.GetProductWithId(request.Id, cancellationToken);
       
         if (product is null)
         {
              return Error.NotFound("ProductNotFound", "Product not found");
         }

         return product;
    }
}