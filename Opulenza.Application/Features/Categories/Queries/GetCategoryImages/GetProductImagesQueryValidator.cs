using FluentValidation;

namespace Opulenza.Application.Features.Categories.Queries.GetCategoryImages;

public class GetCategoryImagesQueryValidator : AbstractValidator<GetCategoryImagesQuery>
{
    public GetCategoryImagesQueryValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotNull().WithMessage("Category id is required")
            .GreaterThan(0).WithMessage("category id must be greater than 0");
            
    }
}