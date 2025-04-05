using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Users.Commands.ChangeUserPassword;

public class ChangeUserPasswordCommandHandler(ICurrentUserProvider currentUserProvider, 
    UserManager<ApplicationUser> userManager, ILogger<ChangeUserPasswordCommandHandler> logger): IRequestHandler<ChangeUserPasswordCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
            var username = currentUserProvider.GetCurrentUser().Username;
            if (string.IsNullOrEmpty(username))
            {
                logger.LogWarning("Username is null or empty");
                return Error.Unauthorized();
            }
        
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                logger.LogWarning("User with username {Username} not found", username);
                return Error.Unauthorized();
            }
        
            if (await userManager.CheckPasswordAsync(user, request.OldPassword) == false)
            {
                logger.LogWarning("Invalid password for user with username {Username}", username);
                return Error.Validation();
            }

            var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (result.Succeeded == false)
            {
                logger.LogError("Password change failed for user with username {Username}, Errors: {Errors}", username, result.Errors);
                return Error.Failure();
            }
        
            return "Password changed successfully";
    }
}