using MediatR;
using Microsoft.AspNetCore.Mvc;
using Opulenza.Api.Endpoints.Internal;
using Opulenza.Api.Helpers;
using Opulenza.Api.Mapping;
using Opulenza.Application.Features.Ratings.Commands.DeleteRating;
using Opulenza.Contracts.Ratings;

namespace Opulenza.Api.Endpoints;

public class RatingEndpoints : CustomEndpoint, IEndpoints
{
    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        // Get ratings for a product
        app.MapGet(ApiEndpoints.Products.Ratings.GetRatings,
                async (int id, [AsParameters] GetRatingsRequest request, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
                {
                    var query = request.MapToGetRatingsQuery(id);
                    var result = await mediator.Send(query, cancellationToken);
                    return result.Match(
                        value => Results.Ok(value.MapToGetRatingsResponse()),
                        Problem);
                })
            .RequireAuthorization()
            .WithMetadata(new ResponseCacheAttribute
            {
                Duration = 60,
                Location = ResponseCacheLocation.Any,
                VaryByQueryKeys = new[] { "Rating" }
            })
            .Produces<GetRatingsResponse>()
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Rating")
;

        // Add a new rating
        app.MapPost(ApiEndpoints.Products.Ratings.AddRating,
                async (int id, [FromBody] AddRatingRequest request, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
                {
                    var command = request.MapToAddRatingCommand(id);
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(
                        _ => Results.NoContent(),
                        Problem);
                })
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Rating")
;

        // Update an existing rating
        app.MapPut(ApiEndpoints.Products.Ratings.UpdateRating,
                async (int id, int ratingId, [FromBody] UpdateRatingRequest request, [FromServices] ISender mediator,
                    CancellationToken cancellationToken) =>
                {
                    var command = request.MapToUpdateRatingCommand(ratingId);
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(
                        _ => Results.NoContent(),
                        Problem);
                })
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Rating")
;

        // Delete a rating
        app.MapDelete(ApiEndpoints.Products.Ratings.DeleteRating,
                async (int id, int ratingId, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
                {
                    var command = new DeleteRatingCommand { RatingId = ratingId };
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(
                        _ => Results.NoContent(),
                        Problem);
                })
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Rating")
;
    }

    public static void AddServices(IServiceCollection services, [FromServices]IConfiguration configuration)
    {
    }
}