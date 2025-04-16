using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Orders.Commands._FulfillCheckout;

public class FulfillCheckoutCommand: IRequest<ErrorOr<string>>
{
    public required string SessionId { get; set; }
}