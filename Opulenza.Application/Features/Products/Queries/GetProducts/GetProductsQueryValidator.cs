using FluentValidation;

namespace Opulenza.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryValidator: AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidator()
    {
        RuleFor(p => p.Brand)
            .MaximumLength(80).WithMessage("Brand name must not exceed 80 characters");
        
        RuleFor(c=>c.Category)
            .MaximumLength(200).WithMessage("Category name must not exceed 200 characters");

        RuleFor(p => p.MinRating)
            .GreaterThanOrEqualTo(1).WithMessage("Min rating must be greater than or equal to 1")
            .LessThanOrEqualTo(5).WithMessage("Min rating must be less than or equal to 5");
        
        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Min price must be greater than or equal to 0")
            .When(x => x.MinPrice.HasValue);
        
        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Max price must be greater than or equal to 0")
            .When(x => x.MaxPrice.HasValue);

        RuleFor(x => x)
            .Must(x => x.MinPrice <= x.MaxPrice)
            .WithMessage("Max price must be greater than or equal min price")
            .When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue);
    }
    
}