using FluentValidation;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Features.Common;

namespace Opulenza.Application.Features.Ratings.Queries.GetRatings;

public class GetRatingsQueryValidator: AbstractValidator<GetRatingsQuery>
{
    public GetRatingsQueryValidator(IProductRepository productRepository)
    {
        Include(new PaginatedQueryValidator());

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product Id is required.")
            .GreaterThan(0).WithMessage("Invalid product id.")
            .MustAsync(async (id, cancellationToken) => await productRepository.ExistsAsync(id, cancellationToken))
            .WithMessage("product not found.");

        RuleFor(x => x.Rating)
            .NotEmpty().WithMessage("Rating is required.")
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
    }
}