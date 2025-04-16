using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Wishlists;

namespace Opulenza.Application.Features.Wishlist.Commands.AddToWishlist;

public class AddToWishlistCommandHandler(ICurrentUserProvider currentUserProvider, 
    IWishlistRepository wishlistRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork): IRequestHandler<AddToWishlistCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(AddToWishlistCommand request, CancellationToken cancellationToken)
    {
        var doesProductExist = await productRepository.ExistsAsync(request.ProductId!.Value, cancellationToken);
        if (doesProductExist == false)
        {
            return Error.NotFound("ProductNotFound", "Product not found");
        }
        
        var userId = currentUserProvider.GetCurrentUser().Id;
        var doesWishListItemExist = await wishlistRepository.ExistsAsync(request.ProductId!.Value, userId);
        if (doesWishListItemExist)
        {
            return Error.Conflict("WishItemAlreadyExists", "This item is already in your wishlist");
        }

        var wishlistItem = new WishListItem()
        {
            UserId = userId,
            ProductId = request.ProductId.Value
        };
        wishlistRepository.Add(wishlistItem);
        await unitOfWork.CommitChangesAsync(cancellationToken);
        
        return wishlistItem.Id;
    }
}