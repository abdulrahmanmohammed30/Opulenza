using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Mapping;

namespace Opulenza.Application.Features.Carts.Queries.GetCart;

public class GetCartQueryHandler(ICurrentUserProvider currentUserProvider, ICartRepository cartRepository): IRequestHandler<GetCartQuery, ErrorOr<GetCartResult>>
{
    public async Task<ErrorOr<GetCartResult>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        var cart = await cartRepository.GetUntrackedCartWithItemsByUserIdAsync(userId, cancellationToken);
        return cart?.MapToGetCartResult()?? new GetCartResult(){
            Items = [],
            TotalPrice = 0,
            TotalPriceAfterDiscount = 0
        };
    }
}