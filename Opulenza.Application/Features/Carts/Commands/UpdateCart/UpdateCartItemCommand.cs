using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Carts.Commands.UpdateCart;

public class UpdateCartItemCommand
{
    public int? ProductId { get; set; }
    public int? Quantity { get; set; }
}