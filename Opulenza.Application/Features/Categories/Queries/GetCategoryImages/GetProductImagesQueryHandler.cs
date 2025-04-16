using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Mapping;

namespace Opulenza.Application.Features.Categories.Queries.GetCategoryImages;

public class GetCategoryImagesQueryHandler(
    ICategoryImageRepository categoryImageRepository,
    ICategoryRepository categoryRepository) : IRequestHandler<GetCategoryImagesQuery, ErrorOr<GetCategoryImagesResult>>
{
    public async Task<ErrorOr<GetCategoryImagesResult>> Handle(GetCategoryImagesQuery request,
        CancellationToken cancellationToken)
    {
        var doesCategoryExist = await categoryRepository.ExistsAsync(request.CategoryId!.Value, cancellationToken);
        if (doesCategoryExist == false)
        {
            return Error.NotFound("CategoryNotFound", $"Category was id {request.CategoryId} does not exist");
        }

        var images = await categoryImageRepository.GetImagesByCategoryId(request.CategoryId.Value, cancellationToken);

        return new GetCategoryImagesResult()
        {
            Images = images.Select(x=>x.MapToImageResult()).ToList()
        };
    }
}