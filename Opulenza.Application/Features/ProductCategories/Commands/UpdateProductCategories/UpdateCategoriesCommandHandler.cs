using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Features.Products.Commands.UpdateProduct;
using Opulenza.Domain.Entities.Categories;

namespace Opulenza.Application.Features.ProductCategories.Commands.UpdateProductCategories;

public class UpdateProductCategoriesCommandHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateProductCommand> logger) : IRequestHandler<UpdateProductCategoriesCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(UpdateProductCategoriesCommand request, CancellationToken cancellationToken)
    {
        // Retrieve the tracked product including its categories.
        var product = await productRepository.GetProductByIdWithCategoriesAsync(request.ProductId!.Value, cancellationToken);
        if (product == null)
        {
            logger.LogWarning("Product with id {ProductId} not found", request.ProductId);
            return Error.NotFound("ProductNotFound", $"Product with Id {request.ProductId} not found");
        }

        // Add categories to the product 
        if (request.Categories != null)
        {
            // Compute existing category IDs.
            var existingCategoryIds = product.Categories?.Select(c => c.Id).ToList() ?? new List<int>();

            // Categories to add: IDs present in the request but not already associated.
            var categoryIdsToAdd = request.Categories.Except(existingCategoryIds).ToList();

            var categoriesToAdd = await categoryRepository.GetCategoriesAsync(categoryIdsToAdd, cancellationToken);

            // Log missing categories if any.
            if (categoryIdsToAdd.Count() != categoriesToAdd.Count)
            {
                var missingCategories = categoryIdsToAdd.Except(categoriesToAdd.Select(c => c.Id));
                logger.LogWarning(
                    "Some categories were not found in the database. Categories that were not found: {Categories}",
                    missingCategories);
            }

            // Include already associated categories that are still in the request.
            var categoriesToKeep = product.Categories?.Where(c => request.Categories.Contains(c.Id)).ToList() ??
                                   new List<Category>();

            // Replace with the new combination.
            product.Categories = categoriesToKeep.Union(categoriesToAdd).ToList();
        }

        // As product is tracked, no explicit Update call is necessary.
        await unitOfWork.CommitChangesAsync(cancellationToken);

        return "Product updated successfully";
    }
}