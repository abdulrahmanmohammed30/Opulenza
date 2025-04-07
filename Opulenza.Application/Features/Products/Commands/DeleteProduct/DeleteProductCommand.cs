using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommand: IRequest<ErrorOr<string>>
{
    public int ProductId { get; init; }
}