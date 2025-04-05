using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Features.Users.Commands.Update;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(
    UserManager<ApplicationUser> userManager,
    ILogger<UpdateUserCommandHandler> logger)
    : IRequestHandler<UpdateUserCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.Username);

        if (user == null)
        {
            logger.LogWarning("User with username {Username} not found", request.Username);
            return Error.Unauthorized();
        }


        if (request.FirstName != null || request.LastName != null)
        {
            if (request.FirstName != null)
            {
                user.FirstName = request.FirstName;
            }

            if (request.LastName != null)
            {
                user.LastName = request.LastName;
            }

            var identityResult = await userManager.UpdateAsync(user);
            if (identityResult.Succeeded == false)
            {
                logger.LogWarning("User update failed for user with username {Username} and Errors: {Errors}",
                    request.Username, identityResult.Errors);
                return Error.Failure(code: "UserUpdateFailed", description: "User update failed");
            }
        }

        return "User updated successfully";
    }
}