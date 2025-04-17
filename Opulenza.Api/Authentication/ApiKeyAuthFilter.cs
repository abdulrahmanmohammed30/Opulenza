using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Opulenza.Application.Authentication;

namespace Opulenza.Api.Authentication;

public class ApiKeyAuthFilterFactoryAttribute : Attribute, IFilterFactory
{
    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetRequiredService<ApiKeyAuthFilter>();
    }

    public bool IsReusable { get; } = false;
}

public class ApiKeyAuthFilter(IConfiguration configuration) : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User.Identity == null || context.HttpContext.User.Identity.IsAuthenticated == false)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName,
                    out var extractedApiKey))
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new UnauthorizedObjectResult("An authorized request.");
            }

            var apiKey = configuration[AuthConstants.ApiKeySectionName];
            if (!apiKey.Equals(extractedApiKey))
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new UnauthorizedObjectResult("An authorized request.");
            }
        }
    }
}
