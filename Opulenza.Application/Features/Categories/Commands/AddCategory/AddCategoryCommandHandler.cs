using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.ServiceContracts;
using Opulenza.Domain.Entities.Categories;

namespace Opulenza.Application.Features.Categories.Commands.AddCategory;

public class AddCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    ICategoryService categoryService,
    IUnitOfWork unitOfWork) : IRequestHandler<AddCategoryCommand, ErrorOr<int>>
{

    public async Task<ErrorOr<int>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {    
        var category = new Category()
        {
            Name = request.Name!,
            Description = request.Description!,
            ParentId = request.ParentId,
            Slug = await categoryService.GenerateUniqueSlugAsync(request.Name!, cancellationToken)
        };

        categoryRepository.Add(category);
        await unitOfWork.CommitChangesAsync(cancellationToken);

        return category.Id;
    }
}