using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Authentication.Commands.ResetPassword;

public class ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager):  IRequestHandler<ResetPasswordCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Error.Validation();
        }

        var isValidToken = await userManager.VerifyUserTokenAsync(user, userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword",
            request.Token);
        if (isValidToken == false)
        {
            return Error.Validation();
        }
        
        var result = await userManager.ResetPasswordAsync(user, request.Token, request.Password);
        if (result.Succeeded == false)
        {
            return Error.Failure();
        }
        
        return "Password reset successfully";
    }
}