using System.Runtime.InteropServices.JavaScript;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Authentication.Commands.RevokeToken;

public class RevokeRefreshTokenCommandHandler(ICurrentUserProvider currentUserProvider, 
    UserManager<ApplicationUser> userManager, ILogger<RevokeRefreshTokenCommandHandler> logger):IRequestHandler<RevokeRefreshTokenCommand,ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var username = currentUserProvider.GetCurrentUser().Username;
        
        var user=await userManager.FindByNameAsync(username);
        if (user == null)
        {
            logger.LogWarning("User with username {Username} not found", username);
            return Error.Unauthorized();
        }
        
        user.RefreshToken = null;
        
        await userManager.UpdateAsync(user);
        
        return "Refresh token revoked successfully";
    }
}