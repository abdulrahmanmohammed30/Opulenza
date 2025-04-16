using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Categories.Queries.GetCategoryImages;

public class GetCategoryImagesQuery: IRequest<ErrorOr<GetCategoryImagesResult>>
{
    public int? CategoryId { get; set; }
}