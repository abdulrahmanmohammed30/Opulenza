using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Mapping;

namespace Opulenza.Application.Features.Wishlist.Queries.GetWishlist;

public class GetWishlistQueryHandler(IWishlistRepository wishlistRepository, ICurrentUserProvider currentUserProvider):IRequestHandler<GetWishlistQuery, ErrorOr<GetWishlistResult>>
{
    public async Task<ErrorOr<GetWishlistResult>> Handle(GetWishlistQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        var wishlistItems = await wishlistRepository.GetWishlistWithProductsAsync(userId, cancellationToken);
        return wishlistItems.MapToGetWishlistResult();
    }
}