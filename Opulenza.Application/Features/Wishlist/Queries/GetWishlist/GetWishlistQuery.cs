using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Wishlist.Queries.GetWishlist;

public class GetWishlistQuery:IRequest<ErrorOr<GetWishlistResult>>
{
}