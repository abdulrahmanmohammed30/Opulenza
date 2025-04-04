using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Authentication.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandle(UserManager<ApplicationUser> userManager): IRequestHandler<ConfirmEmailCommand, ErrorOr<string>> 
{
    public async Task<ErrorOr<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return Error.Unauthorized();
        }

        var result = await userManager.ConfirmEmailAsync(user, request.Token);
        if (result.Succeeded == false)
        {
            return Error.Failure("Email confirmation failed");
        }
        
        return "Email confirmed successfully";
    }
}