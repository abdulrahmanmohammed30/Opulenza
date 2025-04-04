using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Users.Commands.ChangeUserPassword;

public class ChangeUserPasswordCommandHandler(ICurrentUserProvider currentUserProvider, UserManager<ApplicationUser> userManager): IRequestHandler<ChangeUserPasswordCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var username = currentUserProvider.GetCurrentUser().Username;
        if (string.IsNullOrEmpty(username))
        {
            return Error.Unauthorized();
        }
        
        var user = await userManager.FindByNameAsync(username);
        if (user == null)
        {
            return Error.Unauthorized();
        }
        
        if (await userManager.CheckPasswordAsync(user, request.OldPassword) == false)
        {
            return Error.Validation();
        }

        var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (result.Succeeded == false)
        {
            return Error.Failure();
        }
        
        return "Password changed successfully";
    }
}