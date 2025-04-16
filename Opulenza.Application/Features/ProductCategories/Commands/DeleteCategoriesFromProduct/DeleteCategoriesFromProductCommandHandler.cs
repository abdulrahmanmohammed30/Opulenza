using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Features.Products.Commands.UpdateProduct;

namespace Opulenza.Application.Features.ProductCategories.Commands.DeleteCategoriesFromProduct;

public class DeleteCategoriesFromProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateProductCommand> logger) : IRequestHandler<DeleteCategoriesFromProductCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(DeleteCategoriesFromProductCommand request, CancellationToken cancellationToken)
    {
        // Retrieve the tracked product including its categories.
        var product =
            await productRepository.GetProductByIdWithCategoriesAsync(request.ProductId!.Value, cancellationToken);
        if (product == null)
        {
            logger.LogWarning("Product with id {ProductId} not found", request.ProductId);
            return Error.NotFound("ProductNotFound", $"Product with Id {request.ProductId} not found");
        }
        
        if (product.Categories == null || !product.Categories.Any())
        {
            logger.LogWarning("No categories exist for product with id {ProductId}", request.ProductId);
            return Error.NotFound("NoCategories", $"No categories exist for the product with Id {request.ProductId}");
        }
        
        // Add categories to the product 
        if (request.Categories != null)
        {
           product.Categories= product.Categories.Where(c=>request.Categories.Any(categoryId => categoryId == c.Id) == false).ToList();
        }

        await unitOfWork.CommitChangesAsync(cancellationToken);

        return "Product updated successfully";
    }
}
