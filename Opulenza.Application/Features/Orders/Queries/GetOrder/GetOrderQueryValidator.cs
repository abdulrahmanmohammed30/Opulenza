using FluentValidation;

namespace Opulenza.Application.Features.Orders.Queries.GetOrder;

public class GetOrderQueryValidator: AbstractValidator<GetOrderQuery>
{
    public GetOrderQueryValidator()
    {
        RuleFor(x=>x.Id)
            .NotEmpty().WithMessage("Order Id is required.");
    }
}