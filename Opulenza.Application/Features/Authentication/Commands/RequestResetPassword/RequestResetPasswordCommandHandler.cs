using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Opulenza.Application.Common.Utilities;
using Opulenza.Application.Models;
using Opulenza.Application.ServiceContracts;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Authentication.Commands.RequestResetPassword;

public class RequestResetPasswordCommandHandler(
    UserManager<ApplicationUser> userManager,
    IEmailService emailService,
    IUrlGenerator urlGenerator
) : IRequestHandler<RequestResetPasswordCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(RequestResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Error.NotFound();
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        
        var apiEndpoint = urlGenerator.GenerateUrl(action: "ResetPassword", controller: "ResetPassword",
            routeValues: new { token, email = user.Email });

        
        // Construct the email with instructions for calling the API endpoint
        var email = new Email()
        {
            To = request.Email,
            Subject = "Reset Password Request",
            Body = $@"
                        <p>You have requested to reset your password. Since this is an API-based reset, please use the following information to complete the process.</p>
                        <p><strong>Endpoint:</strong> <code>{apiEndpoint}</code></p>
                        <p><strong>HTTP Method:</strong> POST</p>
                        <p>Include the following JSON payload in your request:</p>
                        <pre>
                        {{
                          ""email"": ""{user.Email}"",
                          ""token"": ""{token}"",
                          ""newPassword"": ""YourNewPassword""
                        }}
                        </pre>
                        <p>This token is valid for a limited time. If you did not request a password reset, please ignore this email.</p>",
            IsBodyHtml = true
        };

        await emailService.SendEmailAsync(email);

        return "Reset password instructions sent to your email.";
    }
}