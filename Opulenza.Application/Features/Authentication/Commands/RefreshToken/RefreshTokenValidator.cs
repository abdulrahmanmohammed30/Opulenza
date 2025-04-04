using FluentValidation;

namespace Opulenza.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenValidator: AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(p => p.Token).
            NotEmpty().WithMessage("Token is required.");
        
        RuleFor(p => p.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.")
            .Length(88);
    }
}