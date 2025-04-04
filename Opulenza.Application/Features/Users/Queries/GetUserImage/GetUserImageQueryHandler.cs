using System.Security.Claims;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Opulenza.Application.Common.interfaces;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Users.Queries.GetUserImage;

public class GetUserImageQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IWebHostEnvironment webHostEnvironment,
    UserManager<ApplicationUser> userManager,
    IUserImageRepository userImageRepository)
    : IRequestHandler<GetUserImageQuery, ErrorOr<GetUserImageResult>>
{
    private string GetMimeType(UserImage userImage)
    {
        var extension = Path.GetExtension(userImage.FilePath)?.ToLowerInvariant();

        return extension switch
        {
            ".bmp" => "image/bmp",
            ".gif" => "image/gif",
            ".ico" => "image/x-icon",
            ".jpeg" or ".jpg" => "image/jpeg",
            ".png" => "image/png",
            ".tiff" or ".tif" => "image/tiff",
            ".webp" => "image/webp",
            _ => "application/octet-stream", // Default MIME type for unknown files
        };
    }


    public async Task<ErrorOr<GetUserImageResult>> Handle(GetUserImageQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        var userImage = await userImageRepository.GetByUserIdAsync(userId);
        if (userImage is null)
        {
            return Error.NotFound("User image not found.");
        }

        var physicalImagePath = Path.Combine(webHostEnvironment.WebRootPath, userImage.FilePath);

        var image = await System.IO.File.ReadAllBytesAsync(physicalImagePath, cancellationToken);
        return new GetUserImageResult()
        {
            Image = image,
            MimeType = GetMimeType(userImage)
        };
    }
}