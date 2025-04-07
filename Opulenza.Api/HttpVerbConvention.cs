using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Opulenza.Api;

public class HttpVerbConvention : IActionModelConvention
{
    public void Apply(ActionModel action)
    {
        var httpMethods = action.Attributes.OfType<HttpMethodAttribute>()
            .SelectMany(attr => attr.HttpMethods)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (httpMethods.Any() == false)
        {
            return;
        }

        foreach (var method in httpMethods)
        {
            switch (method.ToUpperInvariant())
            {
                case "GET":
                    if (HasResponse(action, StatusCodes.Status200OK) == false)
                    {
                        action.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status200OK));
                    }
                    action.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails),
                        StatusCodes.Status404NotFound));
                    action.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails),
                        StatusCodes.Status500InternalServerError));
                    break;

                case "POST":
                    if (HasResponse(action, StatusCodes.Status201Created) == false)
                    {
                        action.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status201Created));
                    }
                    
                    action.Filters.Add(new ProducesResponseTypeAttribute(typeof(ValidationProblemDetails),
                        StatusCodes.Status400BadRequest));
                    action.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails),
                        StatusCodes.Status500InternalServerError));
                    break;

                case "PUT":
                    if (HasResponse(action, StatusCodes.Status204NoContent) == false)
                    {
                        action.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status204NoContent));
                    }

                    action.Filters.Add(new ProducesResponseTypeAttribute(typeof(ValidationProblemDetails),
                        StatusCodes.Status400BadRequest));
                    action.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails),
                        StatusCodes.Status500InternalServerError));
                    break;

                case "DELETE":
                    action.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status204NoContent));
                    action.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails),
                        StatusCodes.Status404NotFound));
                    action.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails),
                        StatusCodes.Status500InternalServerError));
                    break;
            }
        }
    }

    private bool HasResponse(ActionModel action, int statusCode)
    {
        return action.Attributes.OfType<ProducesResponseTypeAttribute>()
            .Any(attr => attr.StatusCode == statusCode) ||  
               action.Filters.OfType<ProducesResponseTypeAttribute>()
            .Any(attr => attr.StatusCode == statusCode);
    }
}