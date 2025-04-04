using System.Net;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Opulenza.Api.Mapping;
using Opulenza.Application.Features.Authentication.Commands.ConfirmEmail;
using Opulenza.Application.Features.Authentication.Commands.RevokeToken;
using Opulenza.Application.Features.Authentication.LoginWithGitHubCallback;
using Opulenza.Application.Features.Users.Commands.DeleteUser;
using Opulenza.Contracts.Auth;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Api.Controllers;

[ApiVersion("1.0")]
public class AccountController(
    ISender mediator,
    SignInManager<ApplicationUser> signInManager) : CustomController
{
    [HttpPost(ApiEndpoints.Authentication.Register)]
    public async Task<IActionResult> Register(RegisterRequest registerRequest, CancellationToken cancellationToken)
    {
        var command = registerRequest.MapToRegisterUserCommand();
        var result = await mediator.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    [HttpPost(ApiEndpoints.Authentication.Login)]
    public async Task<IActionResult> Login(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        var query = loginRequest.MapToLoginQuery();
        var result = await mediator.Send(query, cancellationToken);

        return result.Match(value => Ok(value.MapToLoginResponse()), Problem);
    }

    [HttpPost(ApiEndpoints.Authentication.RefreshToken)]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest,
        CancellationToken cancellationToken)
    {
        var command = refreshTokenRequest.MapToRefreshTokenCommand();
        var result = await mediator.Send(command, cancellationToken);

        return result.Match(value => Ok(value.MapToRefreshTokenResponse()), Problem);
    }

    [Authorize]
    [HttpDelete(ApiEndpoints.Authentication.RevokeToken)]
    public async Task<IActionResult> RevokeToken(CancellationToken cancellationToken)
    {
        var command = new RevokeRefreshTokenCommand();
        var result = await mediator.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    [Authorize]
    [HttpPost(ApiEndpoints.Authentication.ChangePassword)]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest changePasswordRequest,
        CancellationToken cancellationToken)
    {
        var command = changePasswordRequest.MapToChangePasswordCommand();
        var result = await mediator.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    //[Authorize]
    [HttpGet(ApiEndpoints.Authentication.ConfirmEmail)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequest changePasswordRequest,
        CancellationToken cancellationToken)
    {
        var command = new ConfirmEmailCommand()
        {
            Token = changePasswordRequest.Token,
            UserId = changePasswordRequest.UserId
        };

        var result = await mediator.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }


    [HttpPost(ApiEndpoints.Authentication.RequestResetPassword)]
    public async Task<IActionResult> RequestResetPassword(SendResetPasswordRequest sendResetPasswordRequest,
        CancellationToken cancellationToken)
    {
        var command = sendResetPasswordRequest.MapToRequestResetPasswordCommand();
        var result = await mediator.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    [HttpPost(ApiEndpoints.Authentication.ResetPassword)]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest,
        CancellationToken cancellationToken)
    {
        var command = resetPasswordRequest.MapToResetPasswordCommand();
        var result = await mediator.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    [HttpGet(ApiEndpoints.Authentication.LoginWithGitHub)]
    public IActionResult LoginWithGitHub(string returnUrl)
    {
        var redirectUrl = Url.Action("LoginWithGitHubCallback", "Account", new { returnUrl });
        
        var properties = signInManager.ConfigureExternalAuthenticationProperties("GitHub", redirectUrl);
        
        return Challenge(properties, "GitHub");
    }


    [HttpGet(ApiEndpoints.Authentication.LoginWithGitHubCallback)]
    public async Task<IActionResult> LoginWithGitHubCallback(string returnUrl = "/", string? remoteError = null)
    {
        var command = new LoginWithGitHubCallbackCommand()
        {
            ReturnUrl = returnUrl,
            RemoteError = remoteError
        };
            
        var result = await mediator.Send(command);

        if (result.IsError)
        {
            if (result.Errors.First().Type.ToString() == "EmailNotConfirmed")
            {
                return Problem(statusCode: 400,
                    detail:
                    "Please verify your email to complete external login linking. A verification email has been sent.");
            }
            
            var errors = string.Join(",", result.Errors.Select(error => error.Description));
            var errorUrl = $"{returnUrl}?error={WebUtility.UrlEncode(errors)}";
            return new RedirectResult(errorUrl); 
        }

        var clientRedirectUrl = $"{returnUrl}?" +
                                $"token={WebUtility.UrlEncode(result.Value.Token)}" +
                                $"&refreshToken={WebUtility.UrlEncode(result.Value.RefreshToken)}" +
                                $"&expiration={WebUtility.UrlEncode(result.Value.Expiration.ToString("o"))}";
        return new RedirectResult(clientRedirectUrl);
    }
 
    
    [Authorize]
    [HttpDelete]
    [Route(ApiEndpoints.Authentication.DeleteAccount)]
    public async Task<IActionResult> DeleteUser()
    {
        var command = new DeleteUserCommand();
        var result = await mediator.Send(command);
        return result.Match(_ => NoContent(), Problem);
    }
}
