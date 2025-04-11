using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommand: IRequest<ErrorOr<string>>
{
    public int? CategoryId { get; set; }
}