using System.Security.Claims;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Common.Utilities;
using Opulenza.Application.Models;
using Opulenza.Application.ServiceContracts;
using Opulenza.Domain.Entities.Carts;
using Opulenza.Domain.Entities.Users;
using Serilog.Context;

namespace Opulenza.Application.Features.Authentication.Commands.LoginWithGitHubCallback;

public class LoginWithGitHubCallbackHandler(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    IAuthenticationService authenticationService,
    IEmailService emailService,
    IUrlGenerator urlGenerator, 
    IRepository<UserImage> userImageRepository,
    ICartRepository cartRepository, 
    IUnitOfWork unitOfWork, 
    ILogger<LoginWithGitHubCallbackHandler> logger)
    : IRequestHandler<LoginWithGitHubCallbackCommand, ErrorOr<ExternalLoginResult>>
{
    public async Task<ErrorOr<ExternalLoginResult>> Handle(LoginWithGitHubCallbackCommand request,
        CancellationToken cancellationToken)
    {
        if (request.RemoteError != null)
        {
            logger.LogWarning("External provider returned an error: {Error}", request.RemoteError);
            return Error.Validation("ExternalProviderRemoteError",
                description: "Error from external provider: " + request.RemoteError);
        }

        var info = await signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            logger.LogWarning("External login information could not be loaded.");
            return Error.Validation("ExternalLoginInfoLoadError",
                description: "Error loading external login information.");
        }

        var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
            isPersistent: false, bypassTwoFactor: true);
        ApplicationUser? user;
        if (result.Succeeded)
        {
            user = (await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey))!;
        }
        else
        {
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var username = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name);
            var avatarUrl = info.Principal.FindFirstValue( "avatar_url");

            if (email == null)
            {
                logger.LogWarning("Email was not received from external provider.");
                return Error.Validation("EmailNotReceivedFromExternalProvider",
                    description: "Email was not received from external provider.");
            }
            
            if (username == null)
            {
                using (LogContext.PushProperty("Username", username))
                {
                    logger.LogWarning("Username was not received from external provider.");
                }
                
                return Error.Validation("UsernameNotReceivedFromExternalProvider",
                    description: "username was not received from external provider.");
            }
            
            if (name == null)
            {
                using (LogContext.Push())
                {
                    logger.LogWarning("Name was not received from external provider.");
                }
                return Error.Validation("NameNotReceivedFromExternalProvider",
                    description: "Name was not received from external provider.");
            }

            // check if the user is already exists but not linked to the external provider
            user = await userManager.FindByEmailAsync(email);

            // If the user exists and the email is verified
            //     -> Link the user to the GitHub directly 

            // if the user exists but the email is not verified
            //     -> Send a confirmation email to the user, return a json response
            //        to the frontend informing them that the user cannot use external providers
            //        until the account is verified 

            if (user is { EmailConfirmed: false })
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                var url = urlGenerator.GenerateUrl("ConfirmEmail",
                    "Account",
                    new
                    {
                        UserId = user.Id,
                        token
                    });
                
                var emailInstance = new Email()
                {
                    To = user.Email, 
                    Subject = "Confirming Email", 
                    Body = $"Please confirm your email by clicking this link: {url}",
                    IsBodyHtml = true
                };
                
                await emailService.SendEmailAsync(emailInstance);

                using (LogContext.PushProperty("UserId", user.Id))
                using (LogContext.PushProperty("username", user.UserName))
                {
                    logger.LogWarning("User email is not confirmed. Email confirmation link sent to {Email}", user.Email);
                }
                return Error.Validation("EmailNotConfirmed",
                    "User email must be confirmed before logging in using an external provider login scheme");
            }
            
            if (user == null)
            {
                // If the user is not found, create a new user
                user = new ApplicationUser()
                {
                    FirstName = name, LastName = "", UserName = username, Email = email,
                };

                var createUserResult = await userManager.CreateAsync(user);
                if (createUserResult.Succeeded == false)
                {
                    using (LogContext.PushProperty("UserInfo", user))
                    {
                        logger.LogWarning("User creation failed: {Errors}", createUserResult.Errors);
                    }
                    return createUserResult.Errors
                        .Select(error => Error.Validation(code: error.Code, description: error.Description))
                        .ToList();
                }
                
                var addToRoleResult = await userManager.AddToRoleAsync(user, "User");
        
                if (addToRoleResult.Succeeded == false)
                {
                    logger.LogWarning("User was not added to role {Role}, Errors: {Errors}", user.UserName, addToRoleResult.Errors);
                    // return roleIdentityResult.Errors.Select(error =>
                    //     Error.Failure(code: error.Code, description: error.Description)).ToList();
                    //return Error.Failure();
                }
        
                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "User"));
                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                await userManager.AddClaimAsync(user, new Claim("username", user.UserName));

                if (string.IsNullOrEmpty(avatarUrl) == false)
                {
                    var userImage = new UserImage()
                    {
                        UserId = user.Id, 
                        FilePath = avatarUrl
                    };

                    userImageRepository.Add(userImage);
                    await unitOfWork.CommitChangesAsync(cancellationToken);
                }
                // create user cart 
                var cart = new Cart()
                {
                    UserId = user.Id
                };
        
                // create user wishlist and cart 
                cartRepository.Add(cart);
                await unitOfWork.CommitChangesAsync(cancellationToken);
            }

            // Link the user to the external login provider 
            var addLoginResult = await userManager.AddLoginAsync(user, info);
            if (!addLoginResult.Succeeded)
            {
                return addLoginResult.Errors
                    .Select(error => Error.Validation(code: error.Code, description: error.Description))
                    .ToList();
            }
        }

        var tokenResult = await authenticationService.GenerateJwt(user);
        return new ExternalLoginResult()
        {
            Token = tokenResult.Token,
            RefreshToken = tokenResult.RefreshToken,
            Expiration = tokenResult.Expiration,
            ReturnUrl = request.ReturnUrl
        };
    }
}
