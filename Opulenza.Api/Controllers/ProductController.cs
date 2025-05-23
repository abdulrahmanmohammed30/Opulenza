﻿using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Opulenza.Api.Mapping;
using Opulenza.Application.Authentication;
using Opulenza.Application.Features.ProductCategories.Queries.GetProductCategories;
using Opulenza.Application.Features.Products.Commands.AddProductImages;
using Opulenza.Application.Features.Products.Commands.DeleteProduct;
using Opulenza.Application.Features.Products.Commands.DeleteProductImage;
using Opulenza.Application.Features.Products.Queries.GetProductById;
using Opulenza.Application.Features.Products.Queries.GetProductBySlug;
using Opulenza.Application.Features.Products.Queries.GetProductImages;
using Opulenza.Contracts.Products;

namespace Opulenza.Api.Controllers;

[ApiVersion("1.0")]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class ProductController(ISender mediator) : CustomController
{
    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpPost]
    [Route(ApiEndpoints.Products.AddProduct)]
    [ProducesResponseType(typeof(AddProductResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddProduct(AddProductRequest request, CancellationToken cancellationToken)
    {
        var command = request.MapToAddProductCommand();
        var result = await mediator.Send(command, cancellationToken);


        return result.Match(
            id => CreatedAtAction(nameof(GetProductById), new { id }, new AddProductResponse
            {
                ProductId = id
            })
            ,
            Problem);
    }

    [HttpGet]
    [Route(ApiEndpoints.Products.GetProductById)]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductById(int id, CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery()
        {
            Id = id
        };

        var result = await mediator.Send(query, cancellationToken);

        return result.Match(product => Ok(product.MapToProductResponse()), Problem);
    }

    [HttpGet]
    [Route(ApiEndpoints.Products.GetProductBySlug)]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductBySlug(string slug, CancellationToken cancellationToken)
    {
        var query = new GetProductBySlugQuery()
        {
            Slug = slug
        };

        var result = await mediator.Send(query, cancellationToken);

        return result.Match(product => Ok(product.MapToProductResponse()), Problem);
    }

    [HttpGet]
    [Route(ApiEndpoints.Products.GetProducts)]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any,
        VaryByQueryKeys =
        [
            "Search", "Brand", "Category", "MinRating", "IsAvailable", "MinPrice", "MaxPrice", "DiscountOnly", "Sort"
        ])]
    [ProducesResponseType(typeof(GetProductListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts([FromQuery] GetProductsRequest request,
        CancellationToken cancellationToken)
    {
        var query = request.MapToGetProductsQuery();

        var results = await mediator.Send(query, cancellationToken);

        return results.Match(
            productList => Ok(productList.MapToGetProductListResponse()),
            Problem);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpPut]
    [Route(ApiEndpoints.Products.UpdateProduct)]
    public async Task<IActionResult> UpdateProduct(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var command = request.MapToUpdateProductRequest();

        var result = await mediator.Send(command, cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete]
    [Route(ApiEndpoints.Products.DeleteProduct)]
    public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand()
        {
            ProductId = id
        };

        var result = await mediator.Send(command, cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpPost]
    [Route(ApiEndpoints.Products.Images.AddImages)]
    [ProducesResponseType(typeof(ProductImagesResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddProductImages(int id, AddProductImagesRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddProductImagesCommand()
        {
            ProductId = id,
            Files = request.Files
        };

        var result = await mediator.Send(command, cancellationToken);

        return result.Match(
            value => Ok(value.MapToImageResponse()),
            Problem);
    }

    [HttpGet]
    [Route(ApiEndpoints.Products.Images.GetImages)]
    [ProducesResponseType(typeof(GetProductImagesResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductImages(int id, CancellationToken cancellationToken)
    {
        var query = new GetProductImagesQuery()
        {
            ProductId = id
        };

        var result = await mediator.Send(query, cancellationToken);

        return result.Match(
            value => Ok(value.MapToGetProductImagesResponse()),
            Problem);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete]
    [Route(ApiEndpoints.Products.Images.DeleteImage)]
    public async Task<IActionResult> DeleteProductImage(int id, int imageId, CancellationToken cancellationToken)
    {
        var command = new DeleteProductImageCommand()
        {
            ProductId = id,
            ImageId = imageId
        };

        var result = await mediator.Send(command, cancellationToken);
        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpGet]
    [Route(ApiEndpoints.Products.Categories.GetCategories)]
    [ProducesResponseType(typeof(GetProductCategoriesResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductCategories([FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var query = new GetProductCategoriesQuery()
        {
            ProductId = id
        };
        var result = await mediator.Send(query, cancellationToken);
        return result.Match(value => Ok(value.MapToGetProductCategoriesResponse()), Problem);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpPost]
    [Route(ApiEndpoints.Products.Categories.AddCategories)]
    [ProducesResponseType(typeof(int), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddCategoriesToProduct([FromRoute] int id,
        [FromBody] AddCategoriesToProductRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.MapToAddCategoriesCommand(id);
        var result = await mediator.Send(command, cancellationToken);
        return result.Match(_ => NoContent(), Problem);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpPut]
    [Route(ApiEndpoints.Products.Categories.UpdateCategories)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateProductCategories([FromRoute] int id,
        [FromBody] UpdateProductCategoriesRequest request, CancellationToken cancellationToken)
    {
        var command = request.MapToUpdateCategoriesCommand(id);
        var result = await mediator.Send(command, cancellationToken);
        return result.Match(_ => NoContent(), Problem);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete]
    [Route(ApiEndpoints.Products.Categories.DeleteCategories)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteCategoriesFromProduct([FromRoute] int id,
        [FromQuery] DeleteCategoriesFromProductRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.MapToDeleteCategoriesCommand(id);
        var result = await mediator.Send(command, cancellationToken);
        return result.Match(_ => NoContent(), Problem);
    }
}