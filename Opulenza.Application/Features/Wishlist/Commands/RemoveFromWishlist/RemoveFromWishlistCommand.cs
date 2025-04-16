using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Wishlist.Commands.RemoveFromWishlist;

public class RemoveFromWishlistCommand: IRequest<ErrorOr<string>>
{
    public int? Id { get; set; }
}