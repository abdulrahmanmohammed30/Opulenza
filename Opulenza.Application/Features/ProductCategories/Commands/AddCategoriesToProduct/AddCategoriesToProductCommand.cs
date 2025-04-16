using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.ProductCategories.Commands.AddCategoriesToProduct;

public class AddCategoriesToProductCommand: IRequest<ErrorOr<string>>
{
    public int? ProductId { get; set; }
    public List<int>? Categories { get; set; }
}