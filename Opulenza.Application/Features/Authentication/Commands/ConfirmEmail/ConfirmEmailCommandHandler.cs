using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Authentication.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandle(
    UserManager<ApplicationUser> userManager,
    ILogger<ConfirmEmailCommandHandle> logger) : IRequestHandler<ConfirmEmailCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            logger.LogWarning("User with ID {UserId} not found", request.UserId);
            return Error.Unauthorized();
        }

        var result = await userManager.ConfirmEmailAsync(user, request.Token);
        if (result.Succeeded == false)
        {
            logger.LogWarning("Email confirmation failed for user with ID {UserId}, Errors: {Errors}", request.UserId, result.Errors);
            return Error.Failure("Email confirmation failed");
        }
        
        return "Email confirmed successfully";
    }
}