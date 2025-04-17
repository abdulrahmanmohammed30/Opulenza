using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Mapping;
using Opulenza.Domain.Entities.Users;
using Serilog;
using IAuthenticationService = Opulenza.Application.ServiceContracts.IAuthenticationService;

namespace Opulenza.Application.Features.Authentication.Queries.Login;

public class LoginQueryHandler(
    UserManager<ApplicationUser> userManager,
    IAuthenticationService authenticationService,
    ILogger<LoginQueryHandler> logger) : IRequestHandler<LoginQuery, ErrorOr<LoginResult>>
{
    public async Task<ErrorOr<LoginResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.Users.Include(u => u.Image)
            .FirstOrDefaultAsync(user => user.UserName == request.Username, cancellationToken: cancellationToken);
        if (user == null)
        {
            logger.LogWarning("User with username {Username} not found", request.Username);
            return Error.Validation("InvalidCredentials", "Invalid username or password");
        }

        if ((await userManager.CheckPasswordAsync(user, request.Password)) == false)
        {
            logger.LogWarning("Invalid password for user with username {Username}", request.Username);
            return Error.Validation("InvalidCredentials", "Invalid username or password");
        }

        if (user.BlockedAt != null)
        {
            if (user.BlockedUntil == null)
            {
                return Error.Validation(
                    code: "Account.PermanentlyBlocked",
                    description: "Account is permanently blocked."
                );
            }

            if (user.BlockedUntil > DateTime.UtcNow)
            {
                return Error.Validation(
                    code: "Account.TemporarilyBlocked",
                    description: $"Account is blocked until {user.BlockedUntil.Value:u}."
                );
            }

            user.BlockedAt = null;
            user.BlockedUntil = null;
            user.BlockedReason = null;
            await userManager.UpdateAsync(user);
        }

        var tokenResult = await authenticationService.GenerateJwt(user);
        return new LoginResult()
        {
            Token = tokenResult.Token,
            RefreshToken = tokenResult.RefreshToken,
            Expiration = tokenResult.Expiration
        };
    }
}