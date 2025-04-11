using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Features.Products.Common;
using Opulenza.Application.ServiceContracts;
using Opulenza.Domain.Entities.Categories;

namespace Opulenza.Application.Features.Categories.Commands.AddCategoryImages;

public class AddCategoryImagesCommandHandler(
    IUploadFileService uploadFileService,
    ICategoryRepository categoryRepository,
    ILogger<AddCategoryImagesCommandHandler> logger,
    ICategoryImageRepository categoryImageRepository,
    IUnitOfWork unitOfWork
)
    : IRequestHandler<AddCategoryImagesCommand, ErrorOr<CategoryImagesResult>>
{
    public async Task<ErrorOr<CategoryImagesResult>> Handle(AddCategoryImagesCommand request,
        CancellationToken cancellationToken)
    {
        var doesCategoryExist = await categoryRepository.ExistsAsync(request.CategoryId.Value, cancellationToken);
        var warnings = new List<string>();

        if (doesCategoryExist == false)
        {
            // NotFound or Bad request considering used provided an id for a Category that doesn't exist
            return Error.NotFound($"Category was id {request.CategoryId.Value} does not exist");
        }

        var directoryPath = $"Categories/{request.CategoryId}/images";

        var categoryImagesList = await Task.WhenAll(request.Files.Select(async (file) =>
        {
            try
            {
                var filePath = await uploadFileService.UploadFile(file, directoryPath, cancellationToken);
                return new CategoryImage()
                {
                    FilePath = filePath,
                    FileName = file.FileName,
                    OriginalFileName = file.FileName,
                    CategoryId = request.CategoryId.Value,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error uploading file {FileName}, Exception: {Exception}", file.FileName, ex);
                warnings.Add("Failed to upload image with name " + file.FileName);
                
                return null;
            }
        }));

        var categoryImages = categoryImagesList.Where(x=> x != null).Select(x=>x!).ToList();

        if (categoryImages.Count > 0)
        {
            logger.LogInformation("Adding {Count} Category images for Category with id {CategoryId}", categoryImages.Count,
                request.CategoryId.Value);
            
            categoryImageRepository.AddImages(categoryImages);
            await unitOfWork.CommitChangesAsync(cancellationToken);
        }
        else
        {
            logger.LogWarning("No Category images were added for Category with id {CategoryId}", request.CategoryId.Value);
            return Error.Failure("No images were uploaded");
        }

        return new CategoryImagesResult()
        {
            CategoryId = request.CategoryId.Value,
            Images = categoryImages.Select(x => new ImageResult()
            {
                Id = x.Id,
                FilePath = x.FilePath,
            }).ToList(),
            Warnings = warnings,
        };
    }
}