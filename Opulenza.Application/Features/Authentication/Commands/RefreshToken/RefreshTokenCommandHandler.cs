using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Opulenza.Application.Features.Authentication.Queries.Login;
using Opulenza.Application.ServiceContracts;
using Opulenza.Application.Settings;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettingsOptions,
    IAuthenticationService authenticationService): 
    IRequestHandler<RefreshTokenCommand,ErrorOr<TokenResult>>
{
    private JwtSettings JwtSettings => jwtSettingsOptions.Value;
    
    // extract user id from the token 
    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer=true, 
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
            // don't tell the user about what has gone wrong because the maybe trying to hack the system
            return Error.Unauthorized();
        }
        
        var user = await userManager.FindByNameAsync(principal.Identity.Name);
        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
        {
            return Error.Unauthorized();
        }

        return await authenticationService.GenerateJwt(user);
    }
}