using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Opulenza.Application.Features.Authentication.Queries.Login;
using Opulenza.Application.ServiceContracts;
using Opulenza.Application.Settings;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Services;

public class AuthenticationService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettingsOptions):IAuthenticationService
{
     private JwtSettings JwtSettings => jwtSettingsOptions.Value;
     
    public string GetRefreshToken()
    {
        var randomNumber = new byte[64];
        
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<TokenResult> GenerateJwt(ApplicationUser? user)
    {
        if (user == null)
        {
            throw new Exception("user was null");
        }
        
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var userRoles = await userManager.GetRolesAsync(user);
        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        
        var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.SecretKey));
        var signingCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddHours(1);
        
        var jwtSecurityToken  = new JwtSecurityToken(
            issuer: JwtSettings.Issuer,
            audience: JwtSettings.Audience,
            claims: claims,
            expires:expiration,
            signingCredentials: signingCredentials);

        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        
        user.RefreshToken = GetRefreshToken();
        user.RefreshTokenExpiry = DateTime.UtcNow.AddHours(24);

        await userManager.UpdateAsync(user);

        return new TokenResult()
        {
            Token = token,
            Expiration = expiration,
            RefreshToken = user.RefreshToken
        };
    }
}