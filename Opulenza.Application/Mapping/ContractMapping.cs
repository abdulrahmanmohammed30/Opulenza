using Opulenza.Application.Features.Categories.Queries.GetCategories;
using Opulenza.Application.Features.Products.Commands.AddProduct;
using Opulenza.Application.Features.Products.Commands.AddProductImages;
using Opulenza.Application.Features.Products.Commands.UpdateProduct;
using Opulenza.Application.Features.Products.Common;
using Opulenza.Application.Features.Ratings.Queries.GetRatings;
using Opulenza.Application.Features.Users.Commands.Create;
using Opulenza.Application.Features.Users.Commands.CreateUser;
using Opulenza.Application.Features.Users.Queries.GetUser;
using Opulenza.Domain.Entities.Categories;
using Opulenza.Domain.Entities.Products;
using Opulenza.Domain.Entities.Ratings;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Mapping;

public static class ContractMapping
{
    public static ApplicationUser MapToApplicationUser(this CreateUserCommand createUserCommand)
    {
        return new ApplicationUser()
        {
            FirstName = createUserCommand.FirstName!,
            LastName = createUserCommand.LastName!,
            UserName = createUserCommand.Username,
            Email = createUserCommand.Email
        };
    }

    public static GetUserQueryResult MapToGetUserResult(this ApplicationUser applicationUser)
    {
        return new GetUserQueryResult()
        {
            Username = applicationUser.UserName,
            FirstName = applicationUser.FirstName,
            LastName = applicationUser.LastName,
            ImageUrl = applicationUser.Image?.FilePath,
            Address = applicationUser.Address?.MapToGetAddressUserResult()
        };
    }

    public static GetUserAddressQueryResult MapToGetAddressUserResult(this UserAddress userAddress)
    {
        return new GetUserAddressQueryResult()
        {
            City = userAddress.City,
            Country = userAddress.Country,
            StreetAddress = userAddress.StreetAddress,
            ZipCode = userAddress.ZipCode,
        };
    }

    public static ProductResult MapToProductResult(this Product p)
    {
        return new ProductResult()
        {
            Id = p.Id,
            Slug = p.Slug,
            Name = p.Name,
            Description = p.Description,
            Brand = p.Brand,
            Tax = p.Tax,
            TaxIncluded = p.TaxIncluded,
            Price = p.Price,
            DiscountPrice = p.DiscountPrice,
            IsAvailable = p.IsAvailable,
            StockQuantity = p.StockQuantity,
            Images = p.Images?.Select(i => new ImageResult()
            {
                Id = i.Id,
                FilePath = i.FilePath,
                IsFeaturedImage = i.IsFeaturedImage,
            }).ToList(),
            Categories = p.Categories?.Select(c => new CategoryResult()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Slug = c.Slug,
                ParentId = c.ParentId
            }).ToList(),
            Ratings = p.Ratings?.Select(r => new RatingResult()
            {
                Value = r.Value,
                UserId = r.UserId,
                ReviewText = r.ReviewText,
            }).ToList()
        };
    }

    public static Dictionary<string, string> ToKeyValueDictionary(this UpdateProductCommand updateProductCommand)
    {
        var dict = new Dictionary<string, string>();

        // Use the null-coalescing operator to handle nullable types.
        dict["Id"] = updateProductCommand.Id?.ToString() ?? string.Empty;
        dict["Name"] = updateProductCommand.Name ?? string.Empty;
        dict["Description"] = updateProductCommand.Description ?? string.Empty;
        dict["Price"] = updateProductCommand.Price?.ToString() ?? string.Empty;
        dict["DiscountPrice"] = updateProductCommand.DiscountPrice?.ToString() ?? string.Empty;
        dict["Tax"] = updateProductCommand.Tax?.ToString() ?? string.Empty;
        dict["TaxIncluded"] = updateProductCommand.TaxIncluded?.ToString() ?? string.Empty;
        dict["Brand"] = updateProductCommand.Brand ?? string.Empty;
        dict["StockQuantity"] = updateProductCommand.StockQuantity?.ToString() ?? string.Empty;

        // Convert the list of categories to a comma-separated string.
        dict["Categories"] = updateProductCommand.Categories != null
            ? string.Join(",", updateProductCommand.Categories)
            : string.Empty;

        return dict;
    }

    public static ImageResult MapToImageResult(this ProductImage productImage)
    {
        return new ImageResult()
        {
            Id = productImage.Id,
            FilePath = productImage.FilePath,
            IsFeaturedImage = productImage.IsFeaturedImage
        };
    }

    public static GetCategoryResult MapToGetCategoryResult(this Category category)
    {
        return new GetCategoryResult()
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Slug = category.Slug,
            ParentId = category.ParentId,
        };
    }
}