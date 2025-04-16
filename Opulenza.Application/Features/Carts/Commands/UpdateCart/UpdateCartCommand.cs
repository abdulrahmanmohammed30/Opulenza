using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Carts.Commands.UpdateCart;

public class UpdateCartCommand: IRequest<ErrorOr<string>>
{
    public List<UpdateCartItemCommand> Items { get; set; } = new(); 
}

