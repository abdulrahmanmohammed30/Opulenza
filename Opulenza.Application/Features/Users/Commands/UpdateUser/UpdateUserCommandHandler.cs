using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Opulenza.Application.Features.Users.Commands.Update;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<UpdateUserCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.Username);

        if (user == null)
            return Error.NotFound(code: "UserNotFound", description: "User not found");


        if (request.FirstName != null || request.LastName != null)
        {
            if (request.FirstName != null) user.FirstName = request.FirstName;
            if (request.LastName != null) user.LastName = request.LastName;
            
            var identityResult = await userManager.UpdateAsync(user);
            if (identityResult.Succeeded == false) return Error.Failure(code: "UserUpdateFailed", description: "User update failed");
        }
        
        return "User updated successfully";
    }
}