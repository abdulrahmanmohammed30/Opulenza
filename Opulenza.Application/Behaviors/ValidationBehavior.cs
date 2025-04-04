using ErrorOr;
using FluentValidation;
using MediatR;

namespace Opulenza.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    :IPipelineBehavior<TRequest, TResponse>
    where TRequest: IRequest<TResponse>
     where TResponse:IErrorOr 
{
    
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {

        var validationResult = await Task.WhenAll(validators.Select(v => v.ValidateAsync(request, cancellationToken)));

        if (validationResult.Any(v=>!v.IsValid))
        {
            return (dynamic) validationResult.SelectMany(v=>v.Errors)
                .Select(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage))
                .ToList();
        }

        var response = await next();

        return response;
    }
} 

