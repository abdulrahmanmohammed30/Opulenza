using FluentValidation;
using Opulenza.Application.Features.Common;

namespace Opulenza.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQueryValidator: AbstractValidator<GetCategoriesQuery>
{
    public GetCategoriesQueryValidator()
    {
        Include(new PaginatedQueryValidator());

        RuleFor(x => x.Search)
            .MaximumLength(200).WithMessage("Search must be less than 200 characters.");
    }
}
