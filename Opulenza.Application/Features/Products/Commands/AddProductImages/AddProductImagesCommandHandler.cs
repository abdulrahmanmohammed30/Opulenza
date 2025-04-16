using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Features.Common;
using Opulenza.Application.Features.Products.Common;
using Opulenza.Application.ServiceContracts;
using Opulenza.Domain.Entities.Products;

namespace Opulenza.Application.Features.Products.Commands.AddProductImages;

public class AddProductImagesCommandHandler(
    IUploadFileService uploadFileService,
    IProductRepository productRepository,
    ILogger<AddProductImagesCommandHandler> logger,
    IProductImageRepository productImageRepository,
    IUnitOfWork unitOfWork
)
    : IRequestHandler<AddProductImagesCommand, ErrorOr<ProductImagesResult>>
{
    public async Task<ErrorOr<ProductImagesResult>> Handle(AddProductImagesCommand request,
        CancellationToken cancellationToken)
    {
        var doesProductExist = await productRepository.ExistsAsync(request.ProductId.Value, cancellationToken);
        var warnings = new List<string>();

        if (doesProductExist == false)
        {
            // NotFound or Bad request considering used provided an id for a product that doesn't exist
            return Error.NotFound($"Product was id {request.ProductId.Value} does not exist");
        }

        var directoryPath = $"products/{request.ProductId}/images";

        var productImagesList = await Task.WhenAll(request.Files.Select(async (file) =>
        {
            try
            {
                var filePath = await uploadFileService.UploadFile(file, directoryPath, cancellationToken);
                return new ProductImage()
                {
                    FilePath = filePath,
                    FileName = file.FileName,
                    OriginalFileName = file.FileName,
                    ProductId = request.ProductId.Value,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error uploading file {FileName}, Exception: {Exception}", file.FileName, ex);
                warnings.Add("Failed to upload image with name " + file.FileName);
                
                return null;
            }
        }));

        var productImages = productImagesList.Where(x=> x != null).Select(x=>x!).ToList();

        if (productImages.Count > 0)
        {

            logger.LogInformation("Adding {Count} product images for product with id {ProductId}", productImages.Count,
                request.ProductId.Value);
            
            productImageRepository.AddImages(productImages);
            await unitOfWork.CommitChangesAsync(cancellationToken);
        }
        else
        {
            logger.LogWarning("No product images were added for product with id {ProductId}", request.ProductId.Value);
            return Error.Failure("No images were uploaded");
        }

        return new ProductImagesResult()
        {
            ProductId = request.ProductId.Value,
            Images = productImages.Select(x => new ImageResult()
            {
                Id = x.Id,
                FilePath = x.FilePath,
            }).ToList(),
            Warnings = warnings,
        };
    }
}