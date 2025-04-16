using System.Security.Claims;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Models;

namespace Opulenza.Api.Services;

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    public CurrentUser GetCurrentUser()
    {
        if (httpContextAccessor?.HttpContext?.User?.Identity?.Name == null)
            throw new ArgumentNullException(nameof(httpContextAccessor));
        
        var userId = httpContextAccessor.HttpContext.User.Claims
            .First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var username = httpContextAccessor.HttpContext.User.Identity.Name;
        var role = 
            httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Role).Value;
        var email = httpContextAccessor.HttpContext.User.Claims.First(c=>c.Type == ClaimTypes.Email).Value;
        
        return new CurrentUser()
        {
             Id = int.Parse(userId),
             Username = username,
             Role = role,
             Email = email
        };
    }
}