using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Features.Common;

namespace Opulenza.Application.Features.ProductCategories.Queries.GetProductCategories;

public class GetProductCategoriesQueryHandler(IProductRepository productRepository): IRequestHandler<GetProductCategoriesQuery, ErrorOr<GetProductCategoriesResult>>
{
    public async Task<ErrorOr<GetProductCategoriesResult>> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        var product =  await productRepository.GetProductByIdWithCategoriesAsync(request.ProductId!.Value, cancellationToken);
        if (product == null)
        {
            return Error.NotFound("ProductNotFound", $"Product with id {request.ProductId} not found.");
        }

        return new GetProductCategoriesResult()
        {
            Categories = product.Categories == null? new List<CategoryResult>():  product.Categories.Select(c => new CategoryResult
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Slug = c.Slug,
                ParentId = c.ParentId,
            }).ToList()
        };
    }
}