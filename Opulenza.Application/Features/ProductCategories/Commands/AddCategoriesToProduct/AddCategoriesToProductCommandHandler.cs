using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Features.ProductCategories.Commands.AddCategoriesToProduct;

namespace Opulenza.Application.Features.ProductCategories.Commands.AddCategories;

public class AddCategoriesCommandHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    ILogger<AddCategoriesCommandHandler> logger,
    IUnitOfWork unitOfWork) : IRequestHandler<AddCategoriesToProductCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(AddCategoriesToProductCommand request, CancellationToken cancellationToken)
    {
        var product =
            await productRepository.GetProductByIdWithCategoriesAsync(request.ProductId!.Value, cancellationToken);
        if (product == null)
        {
            return Error.NotFound("ProductNotFound", $"Product with id {request.ProductId} not found.");
        }

        var existingCategoryIds = product.Categories == null ? [] : product.Categories.Select(c => c.Id).ToList();
        var categoryIdsToAdd =
            request.Categories!.Where(id => existingCategoryIds.Any(categoryId => categoryId == id) == false).ToList();

        // Add categories to the product 
        if (request.Categories != null)
        {
            var categories = await categoryRepository.GetCategoriesAsync(categoryIdsToAdd, cancellationToken);

            if (categoryIdsToAdd.Count != categories.Count)
            {
                var missingCategories = request.Categories.Except(categories.Select(c => c.Id));
                logger.LogWarning(
                    "Some categories were not found in the database. Categories that were not found: {Categories}",
                    missingCategories);
            }

            product.Categories = categories;
        }

        productRepository.Update(product);
        await unitOfWork.CommitChangesAsync(cancellationToken);

        return "Categories added successfully.";
    }
}