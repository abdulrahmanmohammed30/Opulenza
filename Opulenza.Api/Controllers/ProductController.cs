using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Opulenza.Api.Mapping;
using Opulenza.Application.Features.Products.Queries.GetProductBySlug;
using Opulenza.Application.Features.Products.Queries.GetProductId;
using Opulenza.Contracts.Products;

namespace Opulenza.Api.Controllers;

[ApiVersion("1.0")]
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
}