using MediatR;
using Microsoft.AspNetCore.Mvc;
using Opulenza.Api.Endpoints.Internal;
using Opulenza.Api.Helpers;
using Opulenza.Api.Mapping;
using Opulenza.Application.Features.Wishlist.Commands.RemoveFromWishlist;
using Opulenza.Application.Features.Wishlist.Queries.GetWishlist;
using Opulenza.Contracts.Wishlist;

namespace Opulenza.Api.Endpoints;

public class WishlistEndpoints : CustomEndpoint, IEndpoints
{
    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        // Retrieve the current user's wishlist
        app.MapGet(ApiEndpoints.Wishlists.GetWishlist,
                async ([FromServices] ISender mediator
, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(new GetWishlistQuery(), cancellationToken);
                    return result.Match(
                        value => Results.Ok(value.MapToGetWishlistResponse()),
                        Problem);
                })
            .RequireAuthorization()
            .Produces<GetWishlistResponse>()
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Wishlist");

        // Add an item to the wishlist
        app.MapPost(ApiEndpoints.Wishlists.AddToWishlist,
                async ([FromBody] AddToWishlistRequest request, [FromServices] ISender mediator
, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(request.MapToAddToWishlistCommand(), cancellationToken);
                    return result.Match<IResult>(
                        value => Results.Created($"/api/v2{ApiEndpoints.Wishlists.GetWishlist}", value),
                        Problem);
                })
            .RequireAuthorization()
            .Produces<int>(StatusCodes.Status201Created)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Wishlist")
;

        // Remove an item from the wishlist
        app.MapDelete(ApiEndpoints.Wishlists.RemoveFromWishlist,
                async (int id, [FromServices] ISender mediator
, CancellationToken cancellationToken) =>
                {
                    var command = new RemoveFromWishlistCommand { Id = id };
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match<IResult>(
                        _ => Results.NoContent(),
                        Problem);
                })
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Wishlist")
;
    }

    public static void AddServices(IServiceCollection services, [FromServices]IConfiguration configuration)
    {
    }
}