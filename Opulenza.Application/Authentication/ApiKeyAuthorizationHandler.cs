using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Opulenza.Application.Authentication;

public class ApiKeyAuthorizationHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    : AuthorizationHandler<ApiKeyRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            context.Succeed(requirement);
        }
        else
        {
            var extractedApiKey =
                httpContextAccessor?.HttpContext?.Request.Headers[AuthConstants.ApiKeyHeaderName].ToString();
            var apiKey = configuration[AuthConstants.ApiKeySectionName];

            if (extractedApiKey == apiKey)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }

        return Task.CompletedTask;
    }
}