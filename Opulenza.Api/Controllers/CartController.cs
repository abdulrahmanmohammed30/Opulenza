using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Opulenza.Api.Mapping;
using Opulenza.Application.Features.Carts.Queries.GetCart;
using Opulenza.Contracts.Carts;

namespace Opulenza.Api.Controllers;

[ApiVersion("1.0")]
public class CartController(ISender mediator) : CustomController
{
    [Authorize]
    [HttpGet(ApiEndpoints.Carts.GetCart)]
    [ProducesResponseType(typeof(GetCartResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCart()
    {
        var query = new GetCartQuery();
        var result = await mediator.Send(query);
        return result.Match(
            value => Ok(value.MapToGetCartResponse()),
            Problem);

    }

    [Authorize]
    [HttpPost]
    [Route(ApiEndpoints.Carts.UpdateCart)]
    public async Task<IActionResult> UpdateCart([FromBody] UpdateCartRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.MapToUpdateUpdateCartCommand();
        var result = await mediator.Send(command, cancellationToken);
        return result.Match(
            value => NoContent(),
            Problem);
    }
    
}