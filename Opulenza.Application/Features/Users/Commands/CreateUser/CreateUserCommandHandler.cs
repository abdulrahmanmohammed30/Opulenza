using System.Security.Claims;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Common.Utilities;
using Opulenza.Application.Mapping;
using Opulenza.Application.Models;
using Opulenza.Application.ServiceContracts;
using Opulenza.Domain.Entities.Carts;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler(
    IValidator<CreateUserCommand> validator,
    UserManager<ApplicationUser> userManager,
    ICartRepository cartRepository,
    IUnitOfWork unitOfWork,
    IEmailService emailService,
    IUrlGenerator urlGenerator) :
    IRequestHandler<CreateUserCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.IsValid == false)
        {
            return validationResult.Errors
                .Select(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage))
                .ToList();
        }

        var user = request.MapToApplicationUser();
        var identityResult = await userManager.CreateAsync(user, request.Password);

        if (identityResult.Succeeded == false)
        {
            return identityResult.Errors.Select(error =>
                Error.Validation(code: error.Code, description: error.Description)).ToList();
        }

        var roleIdentityResult = await userManager.AddToRoleAsync(user, "User");

        if (roleIdentityResult.Succeeded == false)
        {
            // return roleIdentityResult.Errors.Select(error =>
            //     Error.Failure(code: error.Code, description: error.Description)).ToList();
            //return Error.Failure();
        }

        await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "User"));
        await userManager.AddClaimAsync(user, new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        await userManager.AddClaimAsync(user, new Claim("username", user.UserName));

        // create user cart 
        var cart = new Cart()
        {
            UserId = user.Id
        };

        // create user wishlist and cart 
        cartRepository.Add(cart);
        await unitOfWork.CommitChangesAsync();


        var emailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);

        var emailConfirmationLink = urlGenerator.GenerateUrl(action: "ConfirmEmail", controller: "Account",
            routeValues: new { userId = user.Id, token = emailConfirmationToken });
        
        var email = new Email
        {
            To = user.Email,
            Subject = "Confirm your email",
            Body = $"Please confirm your email by clicking this link: {emailConfirmationLink}",
            IsBodyHtml = true
        };

        await emailService.SendEmailAsync(email);

        return "User created successfully";
    }
}