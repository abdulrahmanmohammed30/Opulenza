using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Products;
using Opulenza.Domain.Entities.Ratings;

namespace Opulenza.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler(
    IProductRepository productRepository,
    IProductSoftDeleteRepository<Rating> ratingProductSoftDeleteRepository,
    IProductSoftDeleteRepository<ProductImage> productImageSoftDeleteRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteProductCommandHandler> logger
) : IRequestHandler<DeleteProductCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetProductByIdWithCategoriesAsync(request.ProductId, cancellationToken);
        if (product == null)
        {
            return Error.NotFound("Product not found");
        }

        try
        {
            product.IsDeleted = true;

            await unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                productRepository.Delete(product);
                await unitOfWork.CommitChangesAsync(cancellationToken);

                // I did not use IUnitOfWord, because I use ExecuteUpdate that sends the SQL UPDATE command directly to the database 
                await ratingProductSoftDeleteRepository.SoftDeleteByUserIdAsync(product.Id);
                await productImageSoftDeleteRepository.SoftDeleteByUserIdAsync(product.Id);
            }, cancellationToken);
            return "Account deleted successfully";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Failed to delete product with Id {product.Id}", product.Id);
            return "Failed to delete user";
        }
    }
}