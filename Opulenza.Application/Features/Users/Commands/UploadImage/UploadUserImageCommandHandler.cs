using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.ServiceContracts;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Application.Features.Users.Commands.UploadImage;

public class UploadUserImageCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IUploadFileService uploadFileService,
    UserManager<ApplicationUser> userManager,
    IRepository<UserImage> repository,
    IUnitOfWork unitOfWork,
    ILogger<UploadUserImageCommandHandler> logger) : IRequestHandler<UploadUserImageCommand, ErrorOr<UploadUserImageResult>>
{
    public async Task<ErrorOr<UploadUserImageResult>> Handle(UploadUserImageCommand request,
        CancellationToken cancellationToken)
    {
        var username = currentUserProvider.GetCurrentUser().Username;

        var user = await userManager.Users.Include(u => u.Image)
            .FirstOrDefaultAsync(u => u.UserName == username, cancellationToken);

        if (user == null)
        {
            logger.LogWarning("User with username {Username} not found", username);
            return Error.Unauthorized();
        }

        var relativeFilePath = $"users/{user.Id}/images";

        var filePath = await uploadFileService.UploadFile(request.File, relativeFilePath, cancellationToken);

        var userImage = new UserImage()
        {
            Id = user.Image?.Id ?? 0,
            UserId = user.Id,
            FilePath = filePath,
        };

        if (user.Image != null)
            repository.Update(userImage);
        else
            repository.Add(userImage);

        await unitOfWork.CommitChangesAsync(cancellationToken);

        return new UploadUserImageResult()
        {
            ImageUrl = filePath
        };
    }
}