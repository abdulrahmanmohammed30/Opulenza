using FluentValidation;

namespace Opulenza.Application.Features.Users.Commands.BlockUser;

public class BlockUserCommandValidator: AbstractValidator<BlockUserCommand>
{
    public BlockUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User id required.");
        
        RuleFor(x=>x.Reason)
            .MaximumLength(600).WithMessage("Reason must be less than 600 characters.");

        RuleFor(x => x.BlockedUntil)
            .Must(blockedUntil => blockedUntil == null || blockedUntil.Value > DateTime.UtcNow);
    }
}
