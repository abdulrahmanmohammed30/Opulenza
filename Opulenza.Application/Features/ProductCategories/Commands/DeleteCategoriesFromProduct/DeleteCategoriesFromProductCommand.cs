using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.ProductCategories.Commands.DeleteCategories;

public class DeleteCategoriesCommand: IRequest<ErrorOr<string>>
{
    public int? ProductId { get; init; }
    public List<int>? Categories { get; init; }
}