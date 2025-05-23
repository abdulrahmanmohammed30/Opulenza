﻿using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Opulenza.Api.Mapping;
using Opulenza.Application.Authentication;
using Opulenza.Application.Features.Users.Commands.BlockUser;
using Opulenza.Application.Features.Users.Commands.UploadImage;
using Opulenza.Application.Features.Users.Queries.GetUser;
using Opulenza.Application.Features.Users.Queries.GetUserAddress;
using Opulenza.Application.Features.Users.Queries.GetUserImage;
using Opulenza.Contracts.Users;


namespace Opulenza.Api.Controllers;

[ApiVersion("1.0")]
public class UserController(ISender mediator) : CustomController
{
    [Authorize]
    [HttpPost(ApiEndpoints.Users.UploadImage)]
    public async Task<IActionResult> UploadImage(IFormFile file, CancellationToken cancellationToken)
    {
        var command = new UploadUserImageCommand()
        {
            File = file
        };

        var result = await mediator.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    [Authorize]
    [HttpGet(ApiEndpoints.Users.PublicImage)]
    public async Task<IActionResult> GetPublicImage(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetUserImageQuery(), cancellationToken);

        if (result.IsError)
        {
            return Problem(result.Errors);
        }

        HttpContext.Response.ContentType = result.Value.MimeType;
        return File(result.Value.Image, result.Value.MimeType);
    }

    [Authorize]
    [HttpPost(ApiEndpoints.Users.UpdateAddress)]
    public async Task<IActionResult> UpdateAddress(UpdateUserAddressRequest updateUserAddressRequest,
        CancellationToken cancellationToken)
    {
        var command = updateUserAddressRequest.MapToUserAddressCommand();
        var result = await mediator.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    [Authorize]
    [ProducesResponseType(typeof(UserAddressResponse), StatusCodes.Status200OK)]
    [HttpGet(ApiEndpoints.Users.GetUserAddress)]
    public async Task<IActionResult> GetUserAddress(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetUserAddressQuery(), cancellationToken);
        return result.Match(value => Ok(value.MapToUserAddressResponse()), Problem);
    }

    [Authorize]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [HttpGet(ApiEndpoints.Users.GetUser)]
    public async Task<IActionResult> GetUser(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetUserQuery(), cancellationToken);
        return result.Match(value => Ok(value.MapToGetUserQuery()), Problem);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    //[ApiKeyAuthFilterFactory]
    //[Authorize(AuthConstants.ApiKeyPolicyName)]
    [HttpGet]
    [ProducesResponseType(typeof(GetUsersResponse), StatusCodes.Status200OK)]
    [Route(ApiEndpoints.Users.GetUsers)]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersRequest request, CancellationToken cancellationToken)
    {
        var command = request.MapToGetUsersQuery();
        var result = await mediator.Send(command, cancellationToken);
        return result.Match(value => Ok(value.MapToGetUsersResponse()), Problem);
    }
    
    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpPost]
    [Route(ApiEndpoints.Users.BlockUser)]
    public async Task<IActionResult> BlockUser(int id, BlockUserRequest request, CancellationToken cancellationToken)
    {
        var command = request.MapToBlockUserCommand(id);
        var result = await mediator.Send(command, cancellationToken);
        return result.Match(_ => NoContent(), Problem);
    }
}