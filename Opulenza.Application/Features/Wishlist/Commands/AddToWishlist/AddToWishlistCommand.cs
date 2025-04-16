using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Wishlist.Commands.AddToWishlist;

public class AddToWishlistCommand: IRequest<ErrorOr<int>>
{   
    public int? ProductId { get; set; }
}