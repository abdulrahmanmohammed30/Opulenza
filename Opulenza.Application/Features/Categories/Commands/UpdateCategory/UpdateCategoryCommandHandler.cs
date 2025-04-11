using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.ServiceContracts;

namespace Opulenza.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    ICategoryService categoryService,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateCategoryCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id!.Value, cancellationToken);
        if (category == null)
        {
            return Error.NotFound("CategoryNotFound", $"Category with id {request.Id} not found.");
        }

        if (category.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase) == false
            && await categoryRepository.ExistsAsync(request.Name!, cancellationToken))
        {
            return Error.Conflict("CategoryAlreadyExists", $"Category with name {request.Name} already exists.");
        }

        if (category.ParentId != request.ParentId)
        {
            //  await categoryRepository.ExistsAsync(request.ParentId!.Value, cancellationToken) == false)
            var parentCategory = await categoryRepository.GetByIdAsync(request.ParentId!.Value, cancellationToken);
            if (parentCategory == null)
            {
                return Error.NotFound("ParentCategoryNotFound", $"Parent category with id {request.ParentId} not found.");
            }

            if (parentCategory.ParentId != null &&
                await categoryRepository.HasAncestorCategory(parentCategory.ParentId.Value, category.Id,
                    cancellationToken))
            {
                return Error.Conflict(
                    "CircularDependency",
                    "Circular dependency detected:  the parent category is a descendent of the specified category, which would create an invalid cycle."
                );
            }
            if (parentCategory.ParentId == category.Id)
            {
                return Error.Conflict(
                    "CircularDependency",
                    "Circular dependency detected: the parent category is already a child of the specified category, which would create an invalid cycle."
                );
            }

            return Error.NotFound("ParentCategoryNotFound", $"Parent category with id {request.ParentId} not found.");
        }

        category.Name = request.Name!;
        category.Description = request.Description!;
        category.ParentId = request.ParentId;

        if (category.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase) == false)
        {
            category.Slug = await categoryService.GenerateUniqueSlugAsync(request.Name!, cancellationToken);
        }

        categoryRepository.Update(category);
        await unitOfWork.CommitChangesAsync(cancellationToken);
        
        return "Category updated successfully.";
    }
}