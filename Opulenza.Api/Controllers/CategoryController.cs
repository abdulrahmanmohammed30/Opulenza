using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Opulenza.Api.Mapping;
using Opulenza.Application.Features.Categories.Commands.AddCategoryImages;
using Opulenza.Application.Features.Categories.Commands.DeleteCategory;
using Opulenza.Application.Features.Categories.Commands.DeleteCategoryImage;
using Opulenza.Application.Features.Categories.Queries.GetCategoryImages;
using Opulenza.Contracts.Categories;
using Opulenza.Contracts.Common;

namespace Opulenza.Api.Controllers;

public class CategoryController(ISender mediator) : CustomController
{
    [HttpGet]
    [Route(ApiEndpoints.Categories.GetCategories)]
    [ProducesResponseType(typeof(GetCategoriesResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories([FromQuery] GetCategoriesRequest request,
        CancellationToken cancellationToken)
    {
        var query = request.MapToGetCategoriesQuery();
        var result = await mediator.Send(query, cancellationToken);
        return result.Match(value => Ok(value.MapToGetCategoriesResponse()), Problem);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpPost]
    [Route(ApiEndpoints.Categories.AddCategory)]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddCategory([FromBody] AddCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.MapToAddCategoryCommand();
        var result = await mediator.Send(command, cancellationToken);
        return result.Match(value =>
            new CreatedAtActionResult(nameof(GetCategories), "Category", null, value), Problem);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpPut]
    [Route(ApiEndpoints.Categories.UpdateCategory)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateCategory([FromRoute] int id,
        [FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = request.MapToUpdateCategoryCommand(id);

        var result = await mediator.Send(command, cancellationToken);
        return result.Match(_ => NoContent(), Problem);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete]
    [Route(ApiEndpoints.Categories.DeleteCategory)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteCategory([FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCategoryCommand()
        {
            CategoryId = id
        };

        var result = await mediator.Send(command, cancellationToken);
        return result.Match(_ => NoContent(), Problem);
    }
    
    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpPost]
    [Route(ApiEndpoints.Categories.Images.AddImages)]
    [ProducesResponseType(typeof(CategoryImagesResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddCategoryImages(int id, AddCategoryImagesRequest request, CancellationToken cancellationToken)
    {
        var command = new AddCategoryImagesCommand()
        {
            CategoryId = id, 
            Files = request.Files
        };

        var result = await mediator.Send(command, cancellationToken);

        return result.Match(
            value => Ok(value.MapToImageResponse() ),
            Problem);
    }
    
    [HttpGet]
    [Route(ApiEndpoints.Categories.Images.GetImages)]
    [ProducesResponseType(typeof(GetCategoryImagesResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategoryImages(int id, CancellationToken cancellationToken)
    {
        var query = new GetCategoryImagesQuery()
        {
            CategoryId = id
        };

        var result = await mediator.Send(query, cancellationToken);

        return result.Match(
            value => Ok(value.MapToGetCategoryImagesResponse()),
            Problem);
    }
    
    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete]
    [Route(ApiEndpoints.Categories.Images.DeleteImage)]
    public async Task<IActionResult> DeleteCategoryImage(int id, int imageId, CancellationToken cancellationToken)
    {
        var command = new DeleteCategoryImageCommand()
        {
            CategoryId = id,
            ImageId = imageId
        };
        
        var result = await mediator.Send(command, cancellationToken);
        return result.Match(
            _ => NoContent(),
            Problem);
    }
}