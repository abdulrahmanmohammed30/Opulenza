using MediatR;
using Microsoft.AspNetCore.Mvc;
using Opulenza.Api.Endpoints.Internal;
using Opulenza.Api.Helpers;
using Opulenza.Api.Mapping;
using Opulenza.Application.Authentication;
using Opulenza.Application.Features.Categories.Commands.AddCategoryImages;
using Opulenza.Application.Features.Categories.Commands.DeleteCategory;
using Opulenza.Application.Features.Categories.Commands.DeleteCategoryImage;
using Opulenza.Application.Features.Categories.Queries.GetCategoryImages;
using Opulenza.Contracts.Categories;

namespace Opulenza.Api.Endpoints;

public class CategoryEndpoints : CustomEndpoint, IEndpoints
{
    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        // Get list of categories
        app.MapGet(ApiEndpoints.Categories.GetCategories,
                async ([AsParameters] GetCategoriesRequest request, [FromServices]ISender mediator, CancellationToken cancellationToken) =>
                {
                    var query = request.MapToGetCategoriesQuery();
                    var result = await mediator.Send(query, cancellationToken);
                    return result.Match(
                        value => Results.Ok(value.MapToGetCategoriesResponse()),
                        Problem);
                })
            .Produces<GetCategoriesResponse>()
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Category")
;

        // Create a new category
        app.MapPost(ApiEndpoints.Categories.AddCategory,
                async ([FromBody] AddCategoryRequest request, [FromServices]ISender mediator, CancellationToken cancellationToken) =>
                {
                    var command = request.MapToAddCategoryCommand();
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(
                        id => Results.Created($"/api/v2{ApiEndpoints.Categories.GetCategories}/{id}", id),
                        Problem);
                })
            .RequireAuthorization(AuthConstants.AdminUserPolicyName)
            .Produces<int>(StatusCodes.Status201Created)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Category")
;

        // Update an existing category
        app.MapPut(ApiEndpoints.Categories.UpdateCategory,
                async (int id, [FromBody] UpdateCategoryRequest request, [FromServices]ISender mediator, CancellationToken cancellationToken) =>
                {
                    var command = request.MapToUpdateCategoryCommand(id);
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(_ => Results.NoContent(), Problem);
                })
            .RequireAuthorization(AuthConstants.AdminUserPolicyName)
            .Produces(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Category")
;

        // Delete a category
        app.MapDelete(ApiEndpoints.Categories.DeleteCategory,
                async (int id, [FromServices]ISender mediator, CancellationToken cancellationToken) =>
                {
                    var command = new DeleteCategoryCommand { CategoryId = id };
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(_ => Results.NoContent(), Problem);
                })
            .RequireAuthorization(AuthConstants.AdminUserPolicyName)
            .Produces(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Category")
;

        // Add images to a category
        app.MapPost(ApiEndpoints.Categories.Images.AddImages,
                async (int id, [FromBody] AddCategoryImagesRequest request, [FromServices]ISender mediator,
                    CancellationToken cancellationToken) =>
                {
                    var command = new AddCategoryImagesCommand
                    {
                        CategoryId = id,
                        Files = request.Files
                    };
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(
                        value => Results.Ok(value.MapToImageResponse()),
                        Problem);
                })
            .RequireAuthorization(AuthConstants.AdminUserPolicyName)
            .Produces<CategoryImagesResponse>(StatusCodes.Status201Created)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Category")
;

        // Get all images for a category
        app.MapGet(ApiEndpoints.Categories.Images.GetImages,
                async (int id, [FromServices]ISender mediator, CancellationToken cancellationToken) =>
                {
                    var query = new GetCategoryImagesQuery { CategoryId = id };
                    var result = await mediator.Send(query, cancellationToken);
                    return result.Match(
                        value => Results.Ok(value.MapToGetCategoryImagesResponse()),
                        Problem);
                })
            .Produces<GetCategoryImagesResponse>()
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Category")
;

        // Delete a specific category image
        app.MapDelete(ApiEndpoints.Categories.Images.DeleteImage,
                async (int id, int imageId, [FromServices]ISender mediator, CancellationToken cancellationToken) =>
                {
                    var command = new DeleteCategoryImageCommand
                    {
                        CategoryId = id,
                        ImageId = imageId
                    };
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(_ => Results.NoContent(), Problem);
                })
            .RequireAuthorization(AuthConstants.AdminUserPolicyName)
            .Produces(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Category")
;
    }

    public static void AddServices(IServiceCollection services, [FromServices]IConfiguration configuration)
    {
    }
}