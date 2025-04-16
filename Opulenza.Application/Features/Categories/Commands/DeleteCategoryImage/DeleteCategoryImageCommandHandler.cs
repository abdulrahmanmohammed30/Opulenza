using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Categories;

namespace Opulenza.Application.Features.Categories.Commands.DeleteCategoryImage;

public class DeleteCategoryImageCommandHandler(
    ICategoryRepository categoryRepository,
    IRepository<CategoryImage> categoryImageRepository, 
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteCategoryImageCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(DeleteCategoryImageCommand request, CancellationToken cancellationToken)
    {
        var doesCategoryExist = await categoryRepository.ExistsAsync(request.CategoryId, cancellationToken);
        if (!doesCategoryExist)
        {
            return Error.NotFound("CategoryNotFound", $"Category with id {request.CategoryId} not found.");
        }

        var image = await categoryImageRepository.GetByIdAsync(request.ImageId, cancellationToken);
        if (image == null)
        {
            return Error.NotFound("ImageNotFound", $"Image with id {request.ImageId} not found.");
        }

        image.IsDeleted = true;
        
        categoryImageRepository.Update(image);
        await unitOfWork.CommitChangesAsync(cancellationToken);
        
        return "Image deleted successfully.";
    }
}