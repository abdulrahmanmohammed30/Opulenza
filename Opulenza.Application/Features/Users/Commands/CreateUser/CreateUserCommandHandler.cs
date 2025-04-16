using System.Security.Claims;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Common.Utilities;
using Opulenza.Application.Mapping;
using Opulenza.Application.Models;
using Opulenza.Application.ServiceContracts;
using Opulenza.Domain.Entities.Carts;
using Opulenza.Domain.Entities.Users;
using Serilog.Context;

namespace Opulenza.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler(
    IValidator<CreateUserCommand> validator,
    UserManager<ApplicationUser> userManager,
    ICartRepository cartRepository,
    IUnitOfWork unitOfWork,
    IEmailService emailService,
    IUrlGenerator urlGenerator,
    IPaymentService paymentService,
    ILogger<CreateUserCommandHandler> logger) :
    IRequestHandler<CreateUserCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        
        var userInfo = new Dictionary<string, string>()
        {
            { "FirstName", request.FirstName },
            { "LastName", request.LastName },
            { "UserName", request.Username },
            { "Email", request.Email },
            { "Password", request.Password },
        };

        if (validationResult.IsValid == false)
        {
            using (LogContext.PushProperty("UserInfo", userInfo))
            {
                logger.LogWarning("Validation failed for CreateUserCommand: {Errors}", validationResult.Errors);
            }
            
            return validationResult.Errors
                .Select(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage))
                .ToList();
        }

        var user = request.MapToApplicationUser();
        var identityResult = await userManager.CreateAsync(user, request.Password);

        if (identityResult.Succeeded == false)
        {
            using (LogContext.PushProperty("UserInfo", userInfo))
            {
                logger.LogWarning("User creation failed: {Errors}", identityResult.Errors);
            }
            return identityResult.Errors.Select(error =>
                Error.Validation(code: error.Code, description: error.Description)).ToList();
        }

        var roleIdentityResult = await userManager.AddToRoleAsync(user, "User");

        if (roleIdentityResult.Succeeded == false)
        {
            // return roleIdentityResult.Errors.Select(error =>
            //     Error.Failure(code: error.Code, description: error.Description)).ToList();
            //return Error.Failure();
            logger.LogWarning("User role assignment failed: {Errors}", roleIdentityResult.Errors);

        }

        await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "User"));
        await userManager.AddClaimAsync(user, new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        await userManager.AddClaimAsync(user, new Claim("username", user.UserName));
        await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, user.Email));

        // create user cart 
        var cart = new Cart()
        {
            UserId = user.Id
        };

        // create user wishlist and cart 
        cartRepository.Add(cart);
        await unitOfWork.CommitChangesAsync(cancellationToken);


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

        try
        {
            // create payment service customer 
          await paymentService.CreateCustomer(user);
        }
        catch (Exception ex)
        {
            logger.LogError("Error creating payment service customer: {Message}", ex.Message);
        }
        
        await emailService.SendEmailAsync(email);

        return "User created successfully";
    }
}