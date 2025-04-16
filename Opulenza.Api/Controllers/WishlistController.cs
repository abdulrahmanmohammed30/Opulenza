using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Opulenza.Api.Mapping;
using Opulenza.Application.Features.Wishlist.Commands.RemoveFromWishlist;
using Opulenza.Application.Features.Wishlist.Queries.GetWishlist;
using Opulenza.Contracts.Wishlist;

namespace Opulenza.Api.Controllers;

[ApiVersion("1.0")]
public class WishlistController(ISender mediator) : CustomController
{
    [Authorize]
    [HttpGet]
    [Route(ApiEndpoints.Wishlists.GetWishlist)]
    [ProducesResponseType(typeof(GetWishlistResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWishlist()
    {
        var result = await mediator.Send(new GetWishlistQuery());
        return result.Match(value => Ok(value.MapToGetWishlistResponse()), Problem);
    }

    [Authorize]
    [HttpPost]
    [Route(ApiEndpoints.Wishlists.AddToWishlist)]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddToWishlist([FromBody] AddToWishlistRequest request)
    {
        var result = await mediator.Send(request.MapToAddToWishlistCommand());
        return result.Match(value => new CreatedAtActionResult(nameof(GetWishlist), "Wishlist"
            , null, value), Problem);
    }

    [Authorize]
    [HttpDelete]
    [Route(ApiEndpoints.Wishlists.RemoveFromWishlist)]
    [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveFromWishlist([FromRoute] int id)
    {
        var command = new RemoveFromWishlistCommand()
        {
            Id = id
        };
        var result = await mediator.Send(command);
        return result.Match(_ => NoContent(), Problem);
    }
}