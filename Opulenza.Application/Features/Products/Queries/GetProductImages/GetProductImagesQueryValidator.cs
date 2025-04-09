using FluentValidation;

namespace Opulenza.Application.Features.Products.Queries.GetProductImages;

public class GetProductImagesQueryValidator : AbstractValidator<GetProductImagesQuery>
{
    public GetProductImagesQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull().WithMessage("ProductId is required")
            .GreaterThan(0).WithMessage("ProductId must be greater than 0");
            
    }
}