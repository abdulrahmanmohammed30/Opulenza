using MediatR;
using Microsoft.AspNetCore.Mvc;
using Opulenza.Api.Endpoints.Internal;
using Opulenza.Api.Helpers;
using Opulenza.Api.Mapping;
using Opulenza.Application.Features.Carts.Queries.GetCart;
using Opulenza.Contracts.Carts;

namespace Opulenza.Api.Endpoints;

public class CartEndpoints : CustomEndpoint, IEndpoints
{
    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        // Get current user's cart
        app.MapGet(ApiEndpoints.Carts.GetCart,
                async ([FromServices]ISender mediator, CancellationToken cancellationToken) =>
                {
                    var query = new GetCartQuery();
                    var result = await mediator.Send(query, cancellationToken);
                    return result.Match(
                        value => Results.Ok(value.MapToGetCartResponse()),
                        Problem);
                })
            .RequireAuthorization()
            .Produces<GetCartResponse>()
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Cart")
;

        // Update current user's cart
        app.MapPost(ApiEndpoints.Carts.UpdateCart,
                async ([FromBody] UpdateCartRequest request, [FromServices]ISender mediator, CancellationToken cancellationToken) =>
                {
                    var command = request.MapToUpdateUpdateCartCommand();
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(
                        _ => Results.NoContent(),
                        Problem);
                })
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Cart")
;
    }

    public static void AddServices(IServiceCollection services, [FromServices]IConfiguration configuration)
    {
    }
}