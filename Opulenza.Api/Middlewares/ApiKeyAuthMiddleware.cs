using Opulenza.Application.Authentication;

namespace Opulenza.Api.Middlewares;

public class ApiKeyAuthMiddleware(RequestDelegate next, IConfiguration configuration)
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.User.Identity == null || context.User.Identity.IsAuthenticated == false)
        {
            if (!context.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("An authorized request.");
                return;
            }
            
            var apiKey = configuration[AuthConstants.ApiKeySectionName];
            if (!apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid Api Key.");
                return;
            }
        }

        await next(context);
    }
}