using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.ProductCategories.Commands.AddCategories;

public class AddCategoriesCommand: IRequest<ErrorOr<string>>
{
    public int? ProductId { get; set; }
    public List<int>? Categories { get; set; }
}