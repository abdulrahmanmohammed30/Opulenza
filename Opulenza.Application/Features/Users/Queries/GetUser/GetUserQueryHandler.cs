using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Mapping;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Users.Queries.GetUser;

public class GetUserQueryHandler(ICurrentUserProvider currentUserProvider, UserManager<ApplicationUser> userManager)
    : IRequestHandler<GetUserQuery, ErrorOr<GetUserQueryResult>>
{
    public async Task<ErrorOr<GetUserQueryResult>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var username = currentUserProvider.GetCurrentUser().Username;

        var user = await userManager.Users.Include(u => u.Image)
            .Include(u => u.Address)
            .FirstOrDefaultAsync(u => u.UserName == username);

        if (user == null)
        {
            return Error.Unauthorized();
        }

        return user.MapToGetUserResult();
    }
}