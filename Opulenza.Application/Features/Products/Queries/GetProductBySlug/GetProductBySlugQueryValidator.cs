using FluentValidation;

namespace Opulenza.Application.Features.Products.Queries.GetProductBySlug;

public class GetProductBySlugQueryValidator: AbstractValidator<GetProductBySlugQuery>
{
    public GetProductBySlugQueryValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required.")
            .MaximumLength(60).WithMessage("Slug must not exceed 100 characters.");
    }
}