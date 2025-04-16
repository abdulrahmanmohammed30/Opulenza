using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Categories.Commands.DeleteCategoryImage;

public class DeleteCategoryImageCommand: IRequest<ErrorOr<string>>
{
    public int CategoryId { get; set; }
    public int ImageId { get; set; }
}