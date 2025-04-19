using Asp.Versioning.Builder;
using ErrorOr;

namespace Opulenza.Api.Endpoints.Internal;

public class CustomEndpoint
{

    protected static IResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Results.Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        return Problem(errors[0]);
    }

    protected static IResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Results.Problem(statusCode: statusCode, detail: error.Description);
    }

    protected static IResult ValidationProblem(List<Error> errors)
    {
        var modelStateDictionary = new List<KeyValuePair<string, string[]>>();

        foreach (var error in errors)
        {
            modelStateDictionary.Add(
                new KeyValuePair<string, string[]>(error.Code, [error.Description])
            );
        }

        return Results.ValidationProblem(modelStateDictionary);
    }
}