using FluentValidation;

namespace Opulenza.Application.Features.Orders.Commands._FulfillCheckout;

public class FulfillCheckoutCommandValidator: AbstractValidator<FulfillCheckoutCommand>
{
    public FulfillCheckoutCommandValidator()
    {
        RuleFor(x => x.SessionId)
            .NotEmpty().WithMessage("Session Id is required.");
    }
}