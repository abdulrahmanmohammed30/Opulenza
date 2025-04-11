using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Mapping;

namespace Opulenza.Application.Features.Products.Queries.GetProductImages;

public class GetProductImagesQueryHandler(
    IProductImageRepository productImageRepository,
    IProductRepository productRepository) : IRequestHandler<GetProductImagesQuery, ErrorOr<GetProductImagesResult>>
{
    public async Task<ErrorOr<GetProductImagesResult>> Handle(GetProductImagesQuery request,
        CancellationToken cancellationToken)
    {
        var doesProductExist = await productRepository.ExistsAsync(request.ProductId.Value, cancellationToken);

        if (doesProductExist == false)
        {
            return Error.NotFound("ProductNotFound", $"Product was id {request.ProductId} does not exist");
        }

        var images = await productImageRepository.GetImagesByProductId(request.ProductId.Value, cancellationToken);

        return new GetProductImagesResult()
        {
            Images = images.Select(x=>x.MapToImageResult()).ToList()
        };
    }
}