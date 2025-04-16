using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;

namespace Opulenza.Application.Features.Wishlist.Commands.RemoveFromWishlist;

public class RemoveFromWishlistCommandHandler(
    IWishlistRepository wishlistRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RemoveFromWishlistCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(RemoveFromWishlistCommand request, CancellationToken cancellationToken)
    {
        var wishlistItem = await wishlistRepository.GetByIdAsync(request.Id!.Value, cancellationToken);
        if (wishlistItem == null)
        {
            return Error.NotFound("WishlistItemNotFound", "Wishlist item not found");
        }

        wishlistRepository.Delete(wishlistItem);
        await unitOfWork.CommitChangesAsync(cancellationToken);

        return "Wishlist item removed successfully";
    }
}