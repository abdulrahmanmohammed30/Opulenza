using FluentValidation;

namespace Opulenza.Application.Features.Products.Queries.GetProductId;

public class GetProductByIdQueryValidator: AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator(){
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Product Id is required")
            .GreaterThan(0).WithMessage("Product Id must be greater than 0"); 
    }
}
