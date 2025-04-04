using Opulenza.Application.Common.Utilities;

namespace Opulenza.Api.Services;

public class UrlGenerator(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor) : IUrlGenerator
{
    public string? GenerateUrl(string action, string controller, object? routeValues)
    {
        var context = httpContextAccessor.HttpContext;

        if (context == null)
        {
            throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext), "HttpContext is null");
        }

        var scheme = context.Request.Scheme;
        var host = context.Request.Host;

        return linkGenerator.GetUriByAction(httpContext: context, action: action, controller: controller,
            values: routeValues,
            scheme: scheme, host: host);
    }
}