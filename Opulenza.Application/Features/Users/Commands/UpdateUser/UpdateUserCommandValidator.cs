using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Opulenza.Application.Features.Users.Commands.Update;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator: AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator(UserManager<ApplicationUser> userManager)
    {
        RuleFor(p => p.Username)
            .NotEmpty();

        // // I don't think this is necessary since the route is already authenticated
        // RuleFor(p => p.Username).MustAsync(async (username, _) => 
        //     await userManager.FindByNameAsync(username) != null).WithMessage("Username doesn't exist.");
    }
}