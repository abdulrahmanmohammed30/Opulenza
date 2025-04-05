using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Opulenza.Application.Features.Authentication.Queries.Login;
using Opulenza.Application.ServiceContracts;
using Opulenza.Application.Settings;
using Opulenza.Domain.Entities.Users;
using Serilog.Context;
using Serilog.Data;

namespace Opulenza.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    UserManager<ApplicationUser> userManager,
    IOptions<JwtSettings> jwtSettingsOptions,
    IAuthenticationService authenticationService,
    ILogger<RefreshTokenCommandHandler> logger) :
    IRequestHandler<RefreshTokenCommand, ErrorOr<TokenResult>>
{
    private JwtSettings JwtSettings => jwtSettingsOptions.Value;

    // extract user id from the token 
    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,

            ValidIssuer = JwtSettings.Issuer,
            ValidAudience = JwtSettings.Audience,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.SecretKey)),
        };

        return new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out _);
    }


    public async Task<ErrorOr<TokenResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // get user identity 
        var principal = GetPrincipalFromExpiredToken(request.Token);

        if (principal?.Identity?.Name is null)
        {
            logger.LogWarning("Failed to extract valid principal from toke. Aborting refresh token process.");
            // don't tell the user about what has gone wrong because they maybe trying to hack the system
            return Error.Unauthorized();
        }

        var user = await userManager.FindByNameAsync(principal.Identity.Name);
        if (user == null)
        {
            logger.LogWarning("User with ID {Username} not found or refresh token is invalid", principal.Identity.Name);
            return Error.Unauthorized();
        }
        
        if (user.RefreshToken != request.RefreshToken)
        {
            logger.LogWarning("Refresh token mismatch for user {UserId}", user.Id);
            return Error.Unauthorized();
        }

        if (user.RefreshTokenExpiry < DateTime.UtcNow)
        {
            using (LogContext.PushProperty("RefreshTokenExpiry", user.RefreshTokenExpiry))
            {
                logger.LogWarning("Refresh token expired for user {UserId}", user.Id);
            }
            return Error.Unauthorized();
        }

        return await authenticationService.GenerateJwt(user);
    }
}