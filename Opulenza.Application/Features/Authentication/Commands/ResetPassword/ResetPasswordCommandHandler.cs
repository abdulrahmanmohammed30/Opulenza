using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Opulenza.Domain.Entities.Users;
using Serilog.Context;

namespace Opulenza.Application.Features.Authentication.Commands.ResetPassword;

public class ResetPasswordCommandHandler(
    UserManager<ApplicationUser> userManager,
    ILogger<ResetPasswordCommandHandler> logger) : IRequestHandler<ResetPasswordCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            logger.LogWarning("User with email {Email} was not found", request.Email);
            return Error.Validation();
        }

        var isValidToken = await userManager.VerifyUserTokenAsync(user,
            userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword",
            request.Token);
        if (isValidToken == false)
        {
            logger.LogWarning("Invalid token for user with email {Email}", request.Email);

            return Error.Validation();
        }

        var result = await userManager.ResetPasswordAsync(user, request.Token, request.Password);
        if (result.Succeeded == false)
        {
            logger.LogWarning("Password reset failed for user with email {Email}", request.Email);
            return Error.Failure();
        }

        return "Password reset successfully";
    }
}