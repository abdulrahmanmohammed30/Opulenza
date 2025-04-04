using FluentValidation;

namespace Opulenza.Application.Features.Authentication.Commands.RequestResetPassword;

public class RequestResetPasswordCommandValidator: AbstractValidator<RequestResetPasswordCommand>
{
    public RequestResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}