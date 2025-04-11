using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Opulenza.Api.Mapping;
using Opulenza.Application.Features.Ratings.Commands.DeleteRating;
using Opulenza.Contracts.Ratings;

namespace Opulenza.Api.Controllers;

[ApiVersion("1.0")]
public class RatingController(ISender mediator): CustomController
{
    
    [HttpGet]
    [Route(ApiEndpoints.Products.Ratings.GetRatings)]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByQueryKeys = ["Rating"])]
    [ProducesResponseType(typeof(GetRatingsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRatings([FromRoute] int id, [FromQuery] GetRatingsRequest request, CancellationToken cancellationToken)
    {
        var query = request.MapToGetRatingsQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return result.Match(value=>Ok(value.MapToGetRatingsResponse()), Problem);
    }
    
    [Authorize]
    [HttpPost]
    [Route(ApiEndpoints.Products.Ratings.AddRating)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddRating([FromRoute] int id, [FromBody] AddRatingRequest request, CancellationToken cancellationToken)
    {
        var command = request.MapToAddRatingCommand(id);
        var result = await mediator.Send(command, cancellationToken);
        return result.Match(_=>NoContent(), Problem);
    }
    
    [Authorize]
    [HttpPut]
    [Route(ApiEndpoints.Products.Ratings.UpdateRating)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateRating([FromRoute] int id, int ratingId, [FromBody] UpdateRatingRequest request, CancellationToken cancellationToken)
    {
        var command = request.MapToUpdateRatingCommand(ratingId);
        var result = await mediator.Send(command, cancellationToken);
        return result.Match(_=>NoContent(), Problem);
    }
    
    [Authorize]
    [HttpDelete]
    [Route(ApiEndpoints.Products.Ratings.DeleteRating)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteRating([FromRoute] int id, int ratingId, CancellationToken cancellationToken)
    {
        var command = new DeleteRatingCommand()
        {
            RatingId = ratingId
        };
        var result = await mediator.Send(command, cancellationToken);
        return result.Match(_=>NoContent(), Problem);
    }
}
