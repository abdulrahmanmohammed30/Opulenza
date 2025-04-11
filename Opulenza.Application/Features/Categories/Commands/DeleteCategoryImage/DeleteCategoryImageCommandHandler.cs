using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Features.Products.Commands.DeleteProductImage;
using Opulenza.Domain.Entities.Products;

namespace Opulenza.Application.Features.Categories.Commands.DeleteCategoryImage;

public class DeleteCategoryImageCommandHandler(
    ICategoryRepository categoryRepository,
    IRepository<ProductImage> categoryImageRepository, 
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductImageCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
    {
        var doesCategoryExist = await categoryRepository.ExistsAsync(request.ProductId, cancellationToken);

        if (!doesCategoryExist)
        {
            return Error.NotFound("CategoryNotFound", $"Category with id {request.ProductId} not found.");
        }

        var image = await categoryImageRepository.GetByIdAsync(request.ImageId, cancellationToken);
        
        if (image == null)
        {
            return Error.NotFound("ImageNotFound", $"Image with id {request.ImageId} not found.");
        }
        
        categoryImageRepository.Update(image);
        await unitOfWork.CommitChangesAsync(cancellationToken);
        
        return "Image deleted successfully.";
    }
}