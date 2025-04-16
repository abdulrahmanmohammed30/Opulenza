using Opulenza.Application.Features.Carts.Queries.GetCart;
using Opulenza.Application.Features.Categories.Queries.GetCategories;
using Opulenza.Application.Features.Common;
using Opulenza.Application.Features.Orders.Queries.GetOrder;
using Opulenza.Application.Features.Orders.Queries.GetOrders;
using Opulenza.Application.Features.Products.Commands.AddProduct;
using Opulenza.Application.Features.Products.Commands.AddProductImages;
using Opulenza.Application.Features.Products.Commands.UpdateProduct;
using Opulenza.Application.Features.Products.Common;
using Opulenza.Application.Features.Ratings.Queries.GetRatings;
using Opulenza.Application.Features.Users.Commands.Create;
using Opulenza.Application.Features.Users.Commands.CreateUser;
using Opulenza.Application.Features.Users.Queries.GetUser;
using Opulenza.Application.Features.Wishlist.Queries.GetWishlist;
using Opulenza.Domain.Entities.Carts;
using Opulenza.Domain.Entities.Categories;
using Opulenza.Domain.Entities.Orders;
using Opulenza.Domain.Entities.Products;
using Opulenza.Domain.Entities.Ratings;
using Opulenza.Domain.Entities.Users;
using Opulenza.Domain.Entities.Wishlists;

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


    public static ImageResult MapToImageResult(this CategoryImage categoryImage)
    {
        return new ImageResult()
        {
            Id = categoryImage.Id,
            FilePath = categoryImage.FilePath,
            IsFeaturedImage = categoryImage.IsFeaturedImage
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

    public static GetCartResult MapToGetCartResult(this Cart cart)
    {
        return new GetCartResult()
        {
            TotalPrice = cart.TotalPrice,
            TotalPriceAfterDiscount = cart.TotalPriceAfterDiscount,
            Items = cart.Items.Select(p => new GetCartItemResult()
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity,
            }).ToList() 
        };
    }

    public static GetWishlistResult MapToGetWishlistResult(this IEnumerable<WishListItem> wishListItems)
    {
        return new GetWishlistResult()
        {
            WishlistItems = wishListItems?.Select(p => new GetWishlistItemResult()
            {
                WishlistItemId = p.Id,
                Id = p.Product!.Id,
                Brand = p.Product!.Brand,
                Description = p.Product!.Description,
                Name = p.Product!.Name, Slug = p.Product!.Slug, 
                IsAvailable = p.Product!.IsAvailable,
                StockQuantity = p.Product!.StockQuantity,
                TaxIncluded = p.Product!.TaxIncluded,
                Tax = p.Product!.Tax,
                DiscountPrice =p.Product!.DiscountPrice,
                Price = p.Product!.Price,
            }).ToList() ?? []
        };
    }

    public static GetOrdersResult MapToGetOrdersResult(this IEnumerable<Order> orders)
    {
        return new GetOrdersResult()
        {
            Orders = orders.Select(o => new GetSingleOrderResult()
            {
                InvoiceUrl = o?.Invoice?.InvoiceUrl,
                OrderStatus = o!.OrderStatus,
                UserId = o?.UserId,
                OrderId = o!.Id,
                PaymentId = o?.PaymentId,
                PaymentMethod = o?.Payment?.PaymentMethod,
                PaymentStatus = o?.Payment?.PaymentStatus,
                TotalAmount = o!.TotalAmount
            }).ToList()
        };
    }

    public static Opulenza.Application.Features.Orders.Queries.GetOrder.GetOrderResult MapToGetOrderResult(
        this Order order)
    {
        return new Opulenza.Application.Features.Orders.Queries.GetOrder.GetOrderResult()
        {
            InvoiceUrl = order?.Invoice?.InvoiceUrl,
            OrderStatus = order!.OrderStatus,
            UserId = order?.UserId,
            OrderId = order!.Id,
            PaymentId = order?.PaymentId,
            PaymentMethod = order?.Payment?.PaymentMethod,
            PaymentStatus = order?.Payment?.PaymentStatus,
            TotalAmount = order!.TotalAmount,
            Items = order.Items!.Select(x => new GetOrderItemResult()
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                TaxIncluded = x.TaxIncluded,
                Tax = x.Tax,
                TotalPrice = x.TotalPrice,
                UnitPrice = x.UnitPrice
            }).ToList()
        };
    }
}