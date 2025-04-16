using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Carts;
using Serilog.Context;

namespace Opulenza.Application.Features.Carts.Commands.UpdateCart;

public class UpdateCartCommandHandler(
    ICurrentUserProvider currentUserProvider,
    ICartRepository cartRepository,
    IProductRepository productRepository,
    ILogger<UpdateCartCommandHandler> logger,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateCartCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        var cart = await cartRepository.GetTrackedCartWithItemsByUserIdAsync(userId, cancellationToken);
        if (cart == null)
        {
            return Error.Failure("FailedToLoadUserCart", "failed to load user cart");
        }

        if (request.Items.Count > 0)
        {
            // remove all items with quantity 0 or items that not present in the request 
            // update items in the database with quantities present in the request  
            // now add all non-zero items that are present in the request but not in the database 
            // if we looped over the items in the request 
            // then go and get the corresponding in the database 
            // if not present
            // if item in the request has quantity greater than zero,then add it to the database
            // if present 
            // if item in the request has quantity that's 0, then remove the item from the database
            // if item in the request has quantity greater than 0, then update the database item quantity 
            // if the request doesn't include some items that are present in the database
            cart.Items = cart.Items
                .Where(existingItem => request.Items.Any(x => x.ProductId == existingItem.ProductId))
                .ToList();

            foreach (var requestItem in request.Items)
            {
                var databaseItem = cart.Items.FirstOrDefault(x => x.ProductId == requestItem.ProductId);
                if (databaseItem == null && requestItem.Quantity > 0)
                {
                    cart.Items.Add(new CartItem()
                    {
                        CartId = cart.Id,
                        ProductId = requestItem.ProductId!.Value,
                        Quantity = requestItem.Quantity!.Value
                    });
                }

                if (databaseItem != null && requestItem.Quantity == 0)
                {
                    cart.Items.Remove(databaseItem);
                }

                if (databaseItem != null && requestItem.Quantity >= 0)
                {
                    databaseItem.Quantity = requestItem.Quantity.Value;
                }
            }
        }
        else
        {
            cart.Items.Clear();
        }

        var productIds = cart.Items.Select(x => x.ProductId).ToList();
        var products = await productRepository.GetDatabaseProductsAsync(productIds, cancellationToken);

        decimal totalPrice = 0;
        decimal totalPriceAfterDiscount = 0;
        foreach (var item in cart.Items)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product == null)
            {
                logger.LogError("Cart contains an item with {CartItemId} that doesn't exist in the database.",
                    item.Id);
                return Error.Validation("CartItemNotFound", $"cart item with id {item.Id} was not found.");
            }

            if (product.StockQuantity == null || item.Quantity > product.StockQuantity)
            {
                using (LogContext.PushProperty("Context", new Dictionary<string, object>
                       {
                           ["CartItemId"] = item.Id,
                           ["RequestedQuantity"] = item.Quantity,
                           ["AvailableStock"] = product.StockQuantity ?? 0
                       }))
                {
                    logger.LogError("Cart item quantity exceeds available stock.");
                    return Error.Validation("CartItemQuantityExceedAvailableStock",
                        "Cart item quantity exceeds available stock.");
                }
            }

            totalPrice += (item.Quantity * product.Price) + (product.TaxIncluded == false
                ? product.Tax * product.Price
                : 0);

            totalPriceAfterDiscount += product.DiscountPrice != null
                ? item.Quantity * product.DiscountPrice.Value + (product.TaxIncluded == false
                    ? product.Tax * product.DiscountPrice.Value
                    : 0)
                : item.Quantity * product.Price + (product.TaxIncluded == false
                    ? product.Tax * product.Price
                    : 0);
        }

        cart.TotalPrice = totalPrice;
        cart.TotalPriceAfterDiscount = totalPriceAfterDiscount;

        await unitOfWork.CommitChangesAsync(cancellationToken);

        return "Updated user cart successfully";
    }
}