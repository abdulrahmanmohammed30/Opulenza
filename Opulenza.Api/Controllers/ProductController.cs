using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Opulenza.Api.Mapping;
using Opulenza.Application.Features.Products.Queries.GetProductQuery;
using Opulenza.Contracts.Products;

namespace Opulenza.Api.Controllers;

[ApiVersion("1.0")]
public class ProductController(ISender mediator) : CustomController
{
    [HttpPost]
    [Route(ApiEndpoints.Products.AddProduct)]
    [ProducesResponseType(typeof(AddProductResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddProduct(AddProductRequest request)
    {
        var command = request.MapToAddProductCommand();
        var result = await mediator.Send(command);
    

        return result.Match(
            id => CreatedAtAction(nameof(GetProduct), new { id }, new AddProductResponse
            {
                ProductId = id
            })
            ,
            Problem);
    }

    [HttpGet]
    [Route(ApiEndpoints.Products.GetProducts)]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    public async Task<ProductResponse> GetProduct(int id)
    {
        var query = new GetProductQuery()
        {
            Id = id 
        };
        
        var result = await mediator.Send(query);
        
        return result.Match(
            product => product.MapToProductResponse(),
            error => throw new Exception(error.ToString())
        );
    }
}