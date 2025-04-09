using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Categories;
using Opulenza.Domain.Entities.Products;

namespace Opulenza.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateProductCommand> logger)
    : IRequestHandler<UpdateProductCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        // Retrieve the tracked product including its categories.
        var product = await productRepository.GetProductByIdWithCategoriesAsync(request.Id.Value, cancellationToken);
        if (product == null)
        {
            logger.LogWarning("Product with id {Id} not found", request.Id);
            return Error.NotFound("ProductNotFound", $"Product with Id {request.Id} not found");
        }

        var priceValidationError =
            ValidatePricing(request.Price, request.DiscountPrice, product.DiscountPrice, product.Price);
        if (priceValidationError is not null)
        {
            return priceValidationError.Value;
        }

        product.Name = request.Name!;
        product.Description = request.Description!;
        product.Brand = request.Brand;
        product.Price = request.Price!.Value;
        product.DiscountPrice = request.DiscountPrice;
        product.Tax = request.Tax!.Value;
        product.TaxIncluded = request.TaxIncluded!.Value;
        product.StockQuantity = request.StockQuantity;
        product.IsAvailable = request.StockQuantity is > 0;
        product.UpdatedAt = DateTime.UtcNow;
        
        // Add categories to the product 
        if (request.Categories != null)
        {
            // Compute existing category IDs.
            var existingCategoryIds = product.Categories?.Select(c => c.Id).ToList() ?? new List<int>();

            // Categories to add: IDs present in the request but not already associated.
            var categoryIdsToAdd = request.Categories.Except(existingCategoryIds);

            var categoriesToAdd = await categoryRepository.GetCategories(categoryIdsToAdd);

            // Log missing categories if any.
            if (request.Categories.Count != categoriesToAdd.Count)
            {
                var missingCategories = request.Categories.Except(categoriesToAdd.Select(c => c.Id));
                logger.LogWarning("Some categories were not found in the database. Categories that were not found: {Categories}", missingCategories);
            }
            
            // Include already associated categories that are still in the request.
            var categoriesToKeep = product.Categories?.Where(c => request.Categories.Contains(c.Id)).ToList() ?? new List<Category>();

            // Replace with the new combination.
            product.Categories = categoriesToKeep.Union(categoriesToAdd).ToList();
        }

        // As product is tracked, no explicit Update call is necessary in most patterns.
        await unitOfWork.CommitChangesAsync(cancellationToken);

        return "Product updated successfully";
    }

    private static ErrorOr<string>? ValidatePricing(decimal? price, decimal? discountPrice,
        decimal? existingDiscountPrice, decimal existingPrice)
    {
        if (price is not null && discountPrice is not null && price < discountPrice)
        {
            return Error.Validation("InvalidDiscountPrice", "Discount price must be less than or equal to the price");
        }

        if (price is not null && existingDiscountPrice is not null && price < existingDiscountPrice)
        {
            return Error.Validation("InvalidPrice", "Price must be greater than or equal to the DiscountPrice");
        }

        if (discountPrice is not null && existingPrice < discountPrice)
        {
            return Error.Validation("InvalidDiscountPrice", "Discount price must be less than or equal to the price");
        }

        return null;
    }
}