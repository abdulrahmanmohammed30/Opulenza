using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Authentication.Queries.Login;

public class LoginValidator : AbstractValidator<LoginQuery>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginValidator(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;

        RuleFor(p => p.Username)
            .NotEmpty().WithMessage("Username is required.");
            // .DependentRules(() =>
            // {
            //     RuleFor(p => p.Username)
            //         .MustAsync(async (username, cancellationToken) =>
            //             await _userManager.FindByNameAsync(username) == null)
            //         .WithMessage("Username is already in use");
            // });
        RuleFor(p => p.Password).NotEmpty().WithMessage("Password is required.");
    }
}