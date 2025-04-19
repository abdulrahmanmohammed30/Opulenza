using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Opulenza.Api.Endpoints.Internal;
using Opulenza.Api.Helpers;
using Opulenza.Api.Mapping;
using Opulenza.Application.Authentication;
using Opulenza.Application.Features.ProductCategories.Commands.DeleteCategoriesFromProduct;
using Opulenza.Application.Features.ProductCategories.Queries.GetProductCategories;
using Opulenza.Application.Features.Products.Commands.AddProductImages;
using Opulenza.Application.Features.Products.Commands.DeleteProduct;
using Opulenza.Application.Features.Products.Commands.DeleteProductImage;
using Opulenza.Application.Features.Products.Queries.GetProductById;
using Opulenza.Application.Features.Products.Queries.GetProductBySlug;
using Opulenza.Application.Features.Products.Queries.GetProductImages;
using Opulenza.Contracts.Products;

namespace Opulenza.Api.Endpoints;

public class ProductEndpoints : CustomEndpoint, IEndpoints
{
    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Products.AddProduct,
            async ([FromBody] AddProductRequest request, [FromServices] ISender mediator, HttpContext context,
                [FromServices] LinkGenerator linkGenerator,
                CancellationToken cancellationToken) =>
            {
                var command = request.MapToAddProductCommand();
                var result = await mediator.Send(command, cancellationToken);

                // var path = linkGenerator.GetPathByName(EndpointNames.GetProduct);
                //  var locationUri = linkGenerator.GetUriByName(context, EndpointNames.GetProduct, new {ProductId = result.Value});
                //  return Results.Created(locationUri, result.Value);
                //     
                    return result.Match(
                        id => Results.CreatedAtRoute(EndpointNames.GetProduct, new { id },
                            new AddProductResponse { ProductId = id }), Problem);
                })
            .RequireAuthorization(AuthConstants.AdminUserPolicyName)
            .Produces<AddProductResponse>(StatusCodes.Status201Created)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Product");

        app.MapGet(ApiEndpoints.Products.GetProductById,
                async (int id, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
                {
                    var query = new GetProductByIdQuery()
                    {
                        Id = id
                    };
                    var result = await mediator.Send(query, cancellationToken);
                    return result.Match(product => Results.Ok(product.MapToProductResponse()), Problem);
                })
            .Produces<ProductResponse>()
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Product")
            .WithName(EndpointNames.GetProduct);

        app.MapGet(ApiEndpoints.Products.GetProductBySlug,
                async (string slug, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
                {
                    var query = new GetProductBySlugQuery() { Slug = slug };
                    var result = await mediator.Send(query, cancellationToken);
                    return result.Match(product => Results.Ok(product.MapToProductResponse()), Problem);
                })
            .Produces<ProductResponse>()
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Product")
            ;

        app.MapGet(ApiEndpoints.Products.GetProducts, async ([FromServices] ISender mediator,
                [AsParameters] GetProductsRequest request,
                CancellationToken cancellationToken) =>
            {
                var query = request.MapToGetProductsQuery();
                var results = await mediator.Send(query, cancellationToken);
                return results.Match(
                    productList => Results.Ok(productList.MapToGetProductListResponse()),
                    Problem);
            })
            .Produces<GetProductListResponse>()
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Product")
            ;

        app.MapPut(ApiEndpoints.Products.UpdateProduct,
                async ([FromBody] UpdateProductRequest request, [FromServices] ISender mediator,
                    CancellationToken cancellationToken) =>
                {
                    var command = request.MapToUpdateProductRequest();
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(
                        _ => Results.NoContent(),
                        Problem);
                })
            .RequireAuthorization(AuthConstants.AdminUserPolicyName)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Product")
            ;

        app.MapDelete(ApiEndpoints.Products.DeleteProduct,
                async (int id, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
                {
                    var command = new DeleteProductCommand() { ProductId = id };
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(
                        _ => Results.NoContent(),
                        Problem);
                })
            .RequireAuthorization(AuthConstants.AdminUserPolicyName)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Product")
            ;

        app.MapPost(ApiEndpoints.Products.Images.AddImages, async (int id, [FromBody] AddProductImagesRequest request,
                [FromServices] ISender mediator,
                CancellationToken cancellationToken) =>
            {
                var command = new AddProductImagesCommand() { ProductId = id, Files = request.Files };
                var result = await mediator.Send(command, cancellationToken);
                return result.Match(
                    value => Results.Ok(value.MapToImageResponse()),
                    Problem);
            })
            .RequireAuthorization(AuthConstants.AdminUserPolicyName)
            .Produces<ProductImagesResponse>(StatusCodes.Status201Created)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Product")
            ;

        app.MapGet(ApiEndpoints.Products.Images.GetImages,
                async (int id, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
                {
                    var query = new GetProductImagesQuery { ProductId = id };
                    var result = await mediator.Send(query, cancellationToken);
                    return result.Match(
                        value => Results.Ok(value.MapToGetProductImagesResponse()),
                        Problem);
                })
            .Produces<GetProductImagesResponse>()
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Product")
            ;

        app.MapDelete(ApiEndpoints.Products.Images.DeleteImage,
                async (int id, int imageId, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
                {
                    var command = new DeleteProductImageCommand
                    {
                        ProductId = id,
                        ImageId = imageId
                    };
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(
                        _ => Results.NoContent(),
                        Problem);
                })
            .RequireAuthorization(AuthConstants.AdminUserPolicyName)
            .Produces(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Product");

        app.MapGet(ApiEndpoints.Products.Categories.GetCategories,
                async (int id, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
                {
                    var query = new GetProductCategoriesQuery { ProductId = id };
                    var result = await mediator.Send(query, cancellationToken);
                    return result.Match(
                        value => Results.Ok(value.MapToGetProductCategoriesResponse()),
                        Problem);
                })
            .Produces<GetProductCategoriesResponse>()
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Product")
            ;

        app.MapPost(ApiEndpoints.Products.Categories.AddCategories,
                async ([FromRoute] int id, [FromBody] AddCategoriesToProductRequest request,
                    [FromServices] ISender mediator,
                    CancellationToken cancellationToken) =>
                {
                    var command = request.MapToAddCategoriesCommand(id);
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(
                        value => Results.Ok(value), // or Results.NoContent() if you prefer
                        Problem);
                })
            .RequireAuthorization(AuthConstants.AdminUserPolicyName)
            .Produces<int>(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Product")
            ;

        app.MapPut(ApiEndpoints.Products.Categories.UpdateCategories,
                [Authorize(AuthConstants.AdminUserPolicyName)]
                //[AllowAnonymous]
                async ([FromRoute] int id, [FromBody] UpdateProductCategoriesRequest request,
                    [FromServices] ISender mediator,
                    CancellationToken cancellationToken) =>
                {
                    var command = request.MapToUpdateCategoriesCommand(id);
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(
                        _ => Results.NoContent(),
                        Problem);
                })
            // .RequireAuthorization(AuthConstants.AdminUserPolicyName)
            // .AllowAnonymous()
            .Produces(StatusCodes.Status204NoContent)
            .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Product")
            ;

    //     app.MapDelete(ApiEndpoints.Products.Categories.DeleteCategories,
    //             async ([FromRoute]int id, [FromQuery]List<int>? categories, [FromServices] ISender mediator,
    //                 CancellationToken cancellationToken) =>
    //             {
    //                 var command = new DeleteCategoriesFromProductCommand()
    //                 {
    //                     ProductId = id,
    //                     Categories = categories
    //                 };
    //                 var result = await mediator.Send(command, cancellationToken);
    //                 return result.Match(
    //                     _ => Results.NoContent(),
    //                     Problem);
    //             })
    //         .RequireAuthorization(AuthConstants.AdminUserPolicyName)
    //         .Produces(StatusCodes.Status204NoContent)
    //         .WithApiVersionSet(ApiVersioningHelpers.ApiVersionSetV2).HasApiVersion(2.0).WithTags("Product")
    //         ;
     }

    public static void AddServices(IServiceCollection services, [FromServices] IConfiguration configuration)
    {
    }
}