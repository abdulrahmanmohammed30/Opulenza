using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Users.Commands.BlockUser;

public class BlockedUserCommandHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<BlockUserCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(BlockUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId!.ToString());
        if (user == null)
        {
            return Error.NotFound("UserNotFound", "user was not found.");
        }

        user.BlockedAt = DateTime.UtcNow;
        user.BlockedReason = request.Reason;
        user.BlockedUntil = request.BlockedUntil;
        await userManager.UpdateAsync(user);
        
        return "";
    }
}