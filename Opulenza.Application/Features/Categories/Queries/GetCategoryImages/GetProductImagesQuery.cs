using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Products.Queries.GetProductImages;

public class GetProductImagesQuery: IRequest<ErrorOr<GetProductImagesResult>>
{
    public int? ProductId { get; set; }
}