using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Features.Products.Common;

namespace Opulenza.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler(IProductRepository productRepository): IRequestHandler<GetProductByIdQuery, ErrorOr<ProductResult>>
{
    public async Task<ErrorOr<ProductResult>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
       var product= await productRepository.GetProductByIdAsync(request.Id, cancellationToken);
       
         if (product is null)
         {
              return Error.NotFound("ProductNotFound", "Product not found");
         }

         return product;
    }
}