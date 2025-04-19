using System.Globalization;
using System.Net;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Opulenza.Api.Endpoints.Internal;
using Opulenza.Api.Helpers;
using Opulenza.Api.Mapping;
using Opulenza.Application.Features.Authentication.Commands.ConfirmEmail;
using Opulenza.Application.Features.Authentication.Commands.LoginWithGitHubCallback;
using Opulenza.Application.Features.Authentication.Commands.RevokeToken;
using Opulenza.Application.Features.Users.Commands.DeleteUser;
using Opulenza.Contracts.Auth;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Api.Endpoints;

public class AccountEndpoints : CustomEndpoint, IEndpoints
{
    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        // Register a new user
        app.MapPost(ApiEndpoints.Authentication.Register,
                async ([FromBody] RegisterRequest registerRequest, [FromServices]ISender mediator, CancellationToken cancellationToken) =>
                {
                    var command = registerRequest.MapToRegisterUserCommand();
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(_ => Results.NoContent(), Problem);
                })
            .Produces(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Account")
;

        // Login and issue tokens
        app.MapPost(ApiEndpoints.Authentication.Login,
                async ([FromBody] LoginRequest loginRequest, [FromServices]ISender mediator, CancellationToken cancellationToken) =>
                {
                    var query = loginRequest.MapToLoginQuery();
                    var result = await mediator.Send(query, cancellationToken);

                    if (result.IsError && result.Errors.First().Type == ErrorType.Unauthorized)
                    {
                        // return 401 Unauthorized
                        return Results.Unauthorized();
                    }

                    return result.Match(
                        value => Results.Ok(value.MapToLoginResponse()),
                        Problem);
                })
            .Produces<LoginResponse>()
            .Produces(StatusCodes.Status401Unauthorized)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Account")
;

        // Refresh JWT using refresh token
        app.MapPost(ApiEndpoints.Authentication.RefreshToken,
                async ([FromBody] RefreshTokenRequest refreshTokenRequest, [FromServices]ISender mediator,
                    CancellationToken cancellationToken) =>
                {
                    var command = refreshTokenRequest.MapToRefreshTokenCommand();
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(
                        value => Results.Ok(value.MapToRefreshTokenResponse()),
                        Problem);
                })
            .Produces<RefreshTokenResponse>()
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Account")
;

        // Revoke current refresh token
        app.MapDelete(ApiEndpoints.Authentication.RevokeToken,
                async ([FromServices]ISender mediator, CancellationToken cancellationToken) =>
                {
                    var command = new RevokeRefreshTokenCommand();
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(_ => Results.NoContent(), Problem);
                })
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Account")
;

        // Change password for authenticated user
        app.MapPost(ApiEndpoints.Authentication.ChangePassword,
                async ([FromBody] ChangePasswordRequest changePasswordRequest, [FromServices]ISender mediator,
                    CancellationToken cancellationToken) =>
                {
                    var command = changePasswordRequest.MapToChangePasswordCommand();
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(_ => Results.NoContent(), Problem);
                })
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Account")
;

        // Confirm email via token & userId in query string
        app.MapGet(ApiEndpoints.Authentication.ConfirmEmail,
                async ([FromBody] ConfirmEmailRequest request, [FromServices]ISender mediator, CancellationToken cancellationToken) =>
                {
                    var command = new ConfirmEmailCommand
                    {
                        Token = request.Token,
                        UserId = request.UserId
                    };
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(_ => Results.NoContent(), Problem);
                })
            .Produces(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Account")
;

        // Send password-reset email
        app.MapPost(ApiEndpoints.Authentication.RequestResetPassword,
                async ([FromBody] SendResetPasswordRequest sendResetPasswordRequest, [FromServices]ISender mediator,
                    CancellationToken cancellationToken) =>
                {
                    var command = sendResetPasswordRequest.MapToRequestResetPasswordCommand();
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(_ => Results.NoContent(), Problem);
                })
            .Produces(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Account")
;

        // Perform password reset
        app.MapPost(ApiEndpoints.Authentication.ResetPassword,
                async ([FromBody] ResetPasswordRequest resetPasswordRequest, [FromServices]ISender mediator,
                    CancellationToken cancellationToken) =>
                {
                    var command = resetPasswordRequest.MapToResetPasswordCommand();
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(_ => Results.NoContent(), Problem);
                })
            .Produces(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Account")
;

        // Start GitHub external login
        app.MapGet(ApiEndpoints.Authentication.LoginWithGitHub,
                (string returnUrl, LinkGenerator linkGenerator, SignInManager<ApplicationUser> signInManager,
                    HttpContext httpContext) =>
                {
                    var callbackUrl = linkGenerator.GetPathByAction(
                        httpContext,
                        action: nameof(ApiEndpoints.Authentication.LoginWithGitHubCallback),
                        controller: "Account",
                        values: new { returnUrl });

                    var props = signInManager.ConfigureExternalAuthenticationProperties("GitHub", callbackUrl);
                    return Results.Challenge(props, ["GitHub"]);
                })
            .Produces(StatusCodes.Status302Found)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Account")
;

        // Handle GitHub callback
        app.MapGet(ApiEndpoints.Authentication.LoginWithGitHubCallback,
                async (string returnUrl, string? remoteError, [FromServices]ISender mediator, CancellationToken cancellationToken) =>
                {
                    var command = new LoginWithGitHubCallbackCommand
                    {
                        ReturnUrl = returnUrl,
                        RemoteError = remoteError
                    };
                    var result = await mediator.Send(command, cancellationToken);

                    if (result.IsError)
                    {
                        if (result.Errors.First().Type == ErrorType.Validation)
                        {
                            // Email not confirmed
                            return Results.Problem(
                                detail:
                                "Please verify your email to complete external login linking. A verification email has been sent.",
                                statusCode: (int)HttpStatusCode.BadRequest);
                        }

                        var errors = string.Join(",", result.Errors.Select(e => e.Description));
                        var errorUrl = $"{returnUrl}?error={WebUtility.UrlEncode(errors)}";
                        return Results.Redirect(errorUrl);
                    }

                    var payloadUrl = $"{returnUrl}?" +
                                     $"token={WebUtility.UrlEncode(result.Value.Token)}&" +
                                     $"refreshToken={WebUtility.UrlEncode(result.Value.RefreshToken)}&" +
                                     $"expiration={WebUtility.UrlEncode(result.Value.Expiration.ToString(CultureInfo.InvariantCulture))}";
                    return Results.Redirect(payloadUrl);
                })
            .Produces(StatusCodes.Status302Found)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Account")
;

        // Delete authenticated user’s account
        app.MapDelete(ApiEndpoints.Authentication.DeleteAccount,
                async ([FromServices]ISender mediator, CancellationToken cancellationToken) =>
                {
                    var command = new DeleteUserCommand();
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(_ => Results.NoContent(), Problem);
                })
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Account")
;
    }

    public static void AddServices(IServiceCollection services, [FromServices]IConfiguration configuration)
    {
    }
}