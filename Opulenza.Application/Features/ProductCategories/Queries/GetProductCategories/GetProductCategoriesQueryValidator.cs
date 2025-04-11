using FluentValidation;

namespace Opulenza.Application.Features.ProductCategories.Queries.GetProductCategories;

public class GetProductCategoriesQueryValidator: AbstractValidator<GetProductCategoriesQuery>
{
    public GetProductCategoriesQueryValidator()
    {
        RuleFor(x=>x.ProductId)
            .NotEmpty().WithMessage("Product Id is required.")
            .GreaterThan(0).WithMessage("Invalid product id.");
    }
}