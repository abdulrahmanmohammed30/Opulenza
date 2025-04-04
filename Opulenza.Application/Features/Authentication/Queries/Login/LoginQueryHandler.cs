using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Opulenza.Application.Mapping;
using Opulenza.Domain.Entities.Users;
using IAuthenticationService = Opulenza.Application.ServiceContracts.IAuthenticationService;

namespace Opulenza.Application.Features.Authentication.Queries.Login;

public class LoginQueryHandler(UserManager<ApplicationUser> userManager, IAuthenticationService authenticationService): IRequestHandler<LoginQuery, ErrorOr<LoginResult>>
{
    public async Task<ErrorOr<LoginResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.Users.Include(u=>u.Image).FirstOrDefaultAsync(user=>user.UserName == request.Username);
        if (user == null)
        {
            return Error.Validation("InvalidCredentials", "Invalid username or password");
        }

        if ((await userManager.CheckPasswordAsync(user, request.Password)) == false)
        {
            return Error.Validation("InvalidCredentials", "Invalid username or password");
        }

        var tokenResult= await authenticationService.GenerateJwt(user);
        return new LoginResult()
        {   
            Token = tokenResult.Token,
            RefreshToken = tokenResult.RefreshToken,
            Expiration = tokenResult.Expiration
        };
    }
}

