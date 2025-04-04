using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Opulenza.Application.Features.Users.Commands.CreateUser;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Users.Commands.Create;

public class CreateUserCommandValidator: AbstractValidator<CreateUserCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;
    public CreateUserCommandValidator(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        SetupStandardValidationRules();
    }

    private void SetupStandardValidationRules()
    {
        RuleFor(p => p.Username)
            .NotEmpty().WithMessage("username is required")
            .MinimumLength(6).WithMessage("username must be at least 6 characters")
            .MaximumLength(12).WithMessage("username must not exceed 12 characters")
            .Matches(@"^[a-zA-Z][a-zA-Z0-9_.]*$")
            .WithMessage(
                "Username must start with a letter and contain only letters, numbers, underscores, and periods.")
            .Must(username => !username.EndsWith("_") && !username.EndsWith("."))
            .WithMessage("Username cannot end with an underscore or period.")
            .DependentRules(() =>
            {       
                RuleFor(p => p.Username)
                .MustAsync(async (username, cancellationToken) => await _userManager.FindByNameAsync(username) == null)
                .WithMessage("Username is already in use");
            });

        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address")
            .DependentRules(() =>
            {
                RuleFor(p => p.Email)
                    .MustAsync(async (email, cancellationToken) => await _userManager.FindByEmailAsync(email) == null)
                    .WithMessage("Email is already in use");
            });
        
        RuleFor(p=>p.Password)
            .NotEmpty().WithMessage("Password is required")
            .Length(6, 16).WithMessage("Password must be between 6 and 16 characters");
        
        RuleFor(p=>p.FirstName)
            .NotEmpty().WithMessage("First name is required");
        
        RuleFor(p=>p.LastName)
            .NotEmpty().WithMessage("Last name is required");
    }
}