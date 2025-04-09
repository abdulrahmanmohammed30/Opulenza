using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Products.Commands.DeleteProductImage;

public class DeleteProductImageCommand: IRequest<ErrorOr<string>>
{
    public int ProductId { get; set; }
    public int ImageId { get; set; }
}