using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.ProductCategories.Queries.GetProductCategories;

public class GetProductCategoriesQuery: IRequest<ErrorOr<GetProductCategoriesResult>>
{
    public int? ProductId { get; set; }
}