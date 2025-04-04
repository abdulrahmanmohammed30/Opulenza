using System.Security.Claims;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Common.Utilities;
using Opulenza.Application.Models;
using Opulenza.Application.ServiceContracts;
using Opulenza.Domain.Entities.Carts;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Authentication.LoginWithGitHubCallback;

public class LoginWithGitHubCallbackHandler(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    IAuthenticationService authenticationService,
    IEmailService emailService,
    IUrlGenerator urlGenerator, 
    IRepository<UserImage> userImageRepository,
    ICartRepository cartRepository, 
    IUnitOfWork unitOfWork)
    : IRequestHandler<LoginWithGitHubCallbackCommand, ErrorOr<ExternalLoginResult>>
{
    public async Task<ErrorOr<ExternalLoginResult>> Handle(LoginWithGitHubCallbackCommand request,
        CancellationToken cancellationToken)
    {
        if (request.RemoteError != null)
        {
            return Error.Validation("ExternalProviderRemoteError",
                description: "Error from external provider: " + request.RemoteError);
        }

        var info = await signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
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
                return Error.Validation("EmailNotReceivedFromExternalProvider",
                    description: "Email was not received from external provider.");
            }
            
            if (username == null)
            {
                return Error.Validation("UsernameNotReceivedFromExternalProvider",
                    description: "username was not received from external provider.");
            }
            
            if (name == null)
            {
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
                    return createUserResult.Errors
                        .Select(error => Error.Validation(code: error.Code, description: error.Description))
                        .ToList();
                }
                
                var addToRoleResult = await userManager.AddToRoleAsync(user, "User");
        
                if (addToRoleResult.Succeeded == false)
                {
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
                    await unitOfWork.CommitChangesAsync();
                }
                // create user cart 
                var cart = new Cart()
                {
                    UserId = user.Id
                };
        
                // create user wishlist and cart 
                cartRepository.Add(cart);
                await unitOfWork.CommitChangesAsync();
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
