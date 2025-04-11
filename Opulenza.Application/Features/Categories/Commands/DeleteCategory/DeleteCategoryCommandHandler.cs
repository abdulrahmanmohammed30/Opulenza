using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;

namespace Opulenza.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork): IRequestHandler<DeleteCategoryCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
       var category = await categoryRepository.GetByIdAsync(request.CategoryId!.Value, cancellationToken);
        if (category == null)
        {
            return Error.NotFound("CategoryNotFound", $"Category with id {request.CategoryId} not found.");
        }

        category.IsDeleted = true;

        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            categoryRepository.Update(category);
            await unitOfWork.CommitChangesAsync(cancellationToken);

           await categoryRepository.DeleteImagesAsync(category.Id, cancellationToken);;
        }, cancellationToken);
        

        return "Category deleted successfully.";
    }
}