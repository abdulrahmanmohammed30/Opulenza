using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Products;

namespace Opulenza.Application.Features.Products.Commands.DeleteProductImage;

public class DeleteProductImageCommandHandler(
    IProductRepository productRepository,
    IRepository<ProductImage> productImageRepository, 
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductImageCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
    {
        var doesProductExist = await productRepository.ExistsAsync(request.ProductId);

        if (!doesProductExist)
        {
            return Error.NotFound("ProductNotFound", $"Product with id {request.ProductId} not found.");
        }

        var image = await productImageRepository.GetByIdAsync(request.ImageId, cancellationToken);
        
        if (image == null)
        {
            return Error.NotFound("ImageNotFound", $"Image with id {request.ImageId} not found.");
        }
        
        image.IsDeleted = true;
        productImageRepository.Update(image);
        await unitOfWork.CommitChangesAsync(cancellationToken);
        
        return "Image deleted successfully.";
    }
}