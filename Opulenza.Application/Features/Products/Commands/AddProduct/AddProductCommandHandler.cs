using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Helpers;
using Opulenza.Application.ServiceContracts;
using Opulenza.Domain.Entities.Products;

namespace Opulenza.Application.Features.Products.Commands.AddProduct;

public class AddProductCommandHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    ILogger<AddProductCommandHandler> logger,
    IPaymentService paymentService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AddProductCommand, ErrorOr<int>>
{
    private async Task<string> GenerateUniqueSlugAsync(string productName, CancellationToken cancellationToken)
    {
        var slug = SlugHelper.GenerateSlug(productName);

        var lastSlug = await productRepository.GetLastSlugWithNameAsync(slug, cancellationToken);

        if (lastSlug == null)
            return slug;

        var slugNumber = lastSlug.Replace(lastSlug, "");

        if (string.IsNullOrWhiteSpace(slugNumber))
            return $"{slug}-1";

        return $"{slug}-{int.Parse(slugNumber.Replace("-", "")) + 1}";
    }

    public async Task<ErrorOr<int>> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        // Create the product 
        // If the process failed, most likely it's on issue on our side, so return failed 
        // otherwise, continue 
        // make sure each category id is valid, before assigning it to the product 
        // sanitize the categories collection and then add it in one go to the database to avoid any performance issues
        // create a repository that takes a collection of ids and product id and assign these categories to the product  

        // generate a unique slug for the product 
        var product = new Product()
        {
            Name = request.Name,
            Description = request.Description,
            Brand = request.Brand,
            Price = request.Price,
            DiscountPrice = request.DiscountPrice,
            Tax = request.Tax,
            TaxIncluded = request.TaxIncluded,
            IsAvailable = request.StockQuantity is > 0,
            StockQuantity = request.StockQuantity,
            Slug = await GenerateUniqueSlugAsync(request.Name, cancellationToken)
        };
        
        // Add categories to the product 
        if (request.Categories != null)
        {
            var categories = await categoryRepository.GetCategoriesAsync(request.Categories,cancellationToken);

            if (request.Categories.Count != categories.Count)
            {
                var missingCategories = request.Categories.Except(categories.Select(c => c.Id));
                logger.LogWarning(
                    "Some categories were not found in the database. Categories that were not found: {Categories}",
                    missingCategories);
            }

            product.Categories = categories;
        }

        string? paymentServiceId = null;
        try
        {
            paymentServiceId = await paymentService.CreateProduct(product);
        }
        catch (Exception ex)
        {
            logger.LogError("Error creating payment service product: {Message}", ex.Message);
        }
        
        product.PaymentServiceId=paymentServiceId;
        productRepository.Add(product);
        await unitOfWork.CommitChangesAsync(cancellationToken);

        return product.Id;
    }
}

