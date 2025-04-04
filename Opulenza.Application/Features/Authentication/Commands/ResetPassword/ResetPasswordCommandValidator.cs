using FluentValidation;

namespace Opulenza.Application.Features.Authentication.Commands.ResetPassword;

public class ResetPasswordCommandValidator: AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("New password is required.")
            .Length(6,16).WithMessage("Password must be between 6 and 16 characters.");;
    }
}