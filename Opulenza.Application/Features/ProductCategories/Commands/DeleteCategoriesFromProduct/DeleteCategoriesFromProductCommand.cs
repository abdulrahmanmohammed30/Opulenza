using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.ProductCategories.Commands.DeleteCategoriesFromProduct;

public class DeleteCategoriesFromProductCommand: IRequest<ErrorOr<string>>
{
    public int? ProductId { get; init; }
    public List<int>? Categories { get; init; }
}