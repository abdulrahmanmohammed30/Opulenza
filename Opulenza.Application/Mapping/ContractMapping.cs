using Opulenza.Application.Features.Products.Commands.AddProduct;
using Opulenza.Application.Features.Products.Queries.Common;
using Opulenza.Application.Features.Users.Commands.Create;
using Opulenza.Application.Features.Users.Commands.CreateUser;
using Opulenza.Application.Features.Users.Queries.GetUser;
using Opulenza.Domain.Entities.Products;
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
}
