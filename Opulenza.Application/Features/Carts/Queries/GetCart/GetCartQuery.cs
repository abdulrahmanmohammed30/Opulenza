using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Carts.Queries.GetCart;

public class GetCartQuery: IRequest<ErrorOr<GetCartResult>>
{
}