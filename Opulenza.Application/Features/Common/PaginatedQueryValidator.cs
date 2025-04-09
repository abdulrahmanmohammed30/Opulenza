using FluentValidation;

namespace Opulenza.Application.Features.Common;

public class PaginatedQueryValidator: AbstractValidator<PaginatedQuery>
{
    public PaginatedQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .NotEmpty().WithMessage("Page number is required.")
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");
        
        RuleFor(x => x.PageSize)
            .NotEmpty().WithMessage("Page size is required.")
            .GreaterThan(0).WithMessage("Page size must be greater than 0.");
    }
}