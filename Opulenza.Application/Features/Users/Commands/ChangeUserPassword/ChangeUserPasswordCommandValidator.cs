using FluentValidation;

namespace Opulenza.Application.Features.Users.Commands.ChangeUserPassword;

public class ChangeUserPasswordCommandValidator: AbstractValidator<ChangeUserPasswordCommand>
{
    public ChangeUserPasswordCommandValidator()
    {
        RuleFor(p=>p.OldPassword)
            .NotEmpty().WithMessage("Old Password is required")
            .Length(6, 16).WithMessage("Invalid Credentials");

        RuleFor(p=>p.NewPassword)
            .NotEmpty().WithMessage("New Password is required")
            .Length(6, 16).WithMessage("New Password must be between 6 and 16 characters");
    }
}