using MediatR;
using Microsoft.AspNetCore.Mvc;
using Opulenza.Api.Endpoints.Internal;
using Opulenza.Api.Helpers;
using Opulenza.Api.Mapping;
using Opulenza.Application.Authentication;
using Opulenza.Application.Features.Users.Commands.BlockUser;
using Opulenza.Application.Features.Users.Commands.UploadImage;
using Opulenza.Application.Features.Users.Queries.GetUser;
using Opulenza.Application.Features.Users.Queries.GetUserAddress;
using Opulenza.Application.Features.Users.Queries.GetUserImage;
using Opulenza.Contracts.Users;

namespace Opulenza.Api.Endpoints;

public class UserEndpoints : CustomEndpoint, IEndpoints
{
    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        // Upload a user image
        app.MapPost(ApiEndpoints.Users.UploadImage,
                async ([FromServices]IFormFile file, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
                {
                    var command = new UploadUserImageCommand { File = file };
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match<IResult>(
                        _ => Results.NoContent(),
                        Problem);
                })
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("User")
;

        // Get the authenticated user’s public image
        app.MapGet(ApiEndpoints.Users.PublicImage,
                async ([FromServices] ISender mediator, HttpContext httpContext, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(new GetUserImageQuery(), cancellationToken);
                    if (result.IsError)
                        return Problem(result.Errors);

                    httpContext.Response.ContentType = result.Value.MimeType;
                    return Results.File(result.Value.Image, result.Value.MimeType);
                })
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("User")
;

        // Update the authenticated user’s address
        app.MapPost(ApiEndpoints.Users.UpdateAddress,
                async ([FromBody] UpdateUserAddressRequest request, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
                {
                    var command = request.MapToUserAddressCommand();
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match<IResult>(
                        _ => Results.NoContent(),
                        Problem);
                })
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("User")
;

        // Get the authenticated user’s address
        app.MapGet(ApiEndpoints.Users.GetUserAddress,
                async ([FromServices] ISender mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(new GetUserAddressQuery(), cancellationToken);
                    return result.Match<IResult>(
                        value => Results.Ok(value.MapToUserAddressResponse()),
                        Problem);
                })
            .RequireAuthorization()
            .Produces<UserAddressResponse>(StatusCodes.Status200OK)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("User")
;

        // Get the authenticated user’s profile
        app.MapGet(ApiEndpoints.Users.GetUser,
                async ([FromServices] ISender mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(new GetUserQuery(), cancellationToken);
                    return result.Match<IResult>(
                        value => Results.Ok(value.MapToGetUserQuery()),
                        Problem);
                })
            .RequireAuthorization()
            .Produces<UserResponse>(StatusCodes.Status200OK)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("User")
;

        // Get all users (admin only)
        app.MapGet(ApiEndpoints.Users.GetUsers,
                async ([FromBody] GetUsersRequest request, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
                {
                    var query = request.MapToGetUsersQuery();
                    var result = await mediator.Send(query, cancellationToken);
                    return result.Match<IResult>(
                        value => Results.Ok(value.MapToGetUsersResponse()),
                        Problem);
                })
            .RequireAuthorization(AuthConstants.AdminUserPolicyName)
            .Produces<GetUsersResponse>(StatusCodes.Status200OK)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("User")
;

        // Block a user (admin only)
        app.MapPost(ApiEndpoints.Users.BlockUser,
                async (int id, [FromBody] BlockUserRequest request, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
                {
                    var command = request.MapToBlockUserCommand(id);
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match<IResult>(
                        _ => Results.NoContent(),
                        Problem);
                })
            .RequireAuthorization(AuthConstants.AdminUserPolicyName)
            .Produces(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("User")
;
    }

    public static void AddServices(IServiceCollection services, [FromServices]IConfiguration configuration)
    {
    }
}