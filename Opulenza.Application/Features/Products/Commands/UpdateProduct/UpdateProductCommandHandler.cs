using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
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
        var product = await productRepository.GetProductByIdAsync(request.Id.Value, cancellationToken);


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

        var updatedProduct = new Product()
        {
            Id = product.Id,
            Slug = product.Slug,
            Name = request.Name ?? product.Name,
            Description = request.Description ?? product.Description,
            Brand = request.Brand ?? product.Brand,
            Price = request.Price ?? product.Price,
            DiscountPrice = request.DiscountPrice ?? product.DiscountPrice,
            Tax = request.Tax ?? product.Tax,
            TaxIncluded = request.TaxIncluded ?? product.TaxIncluded,
            StockQuantity = request.StockQuantity ?? product.StockQuantity,
            IsAvailable = request.StockQuantity == null ? product.IsAvailable : request.StockQuantity is > 0,
            UpdatedAt = DateTime.UtcNow,
        };

        // Add categories to the product 
        if (request.Categories != null)
        {
            var existingCategories = product.Categories == null ? [] : product.Categories.Select(c => c.Id).ToList();

            // categories to add are the categories that the request has but the product doesn't  
            var categoriesToAdd =
                request.Categories.Where(categoryId => existingCategories.Contains(categoryId) == false);

            var categories = await categoryRepository.GetCategories(categoriesToAdd);

            if (request.Categories.Count != categories.Count)
            {
                var missingCategories = request.Categories.Except(categories.Select(c => c.Id));
                logger.LogWarning(
                    "Some categories were not found in the database. Categories that were not found: {Categories}",
                    missingCategories);
            }

            foreach (var category in product.Categories ?? [])
            {
                if (request.Categories.Contains(category.Id))
                    categories.Add(category);
            }

            product.Categories = categories;
        }

        productRepository.Update(updatedProduct);
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