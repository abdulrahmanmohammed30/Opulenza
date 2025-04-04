using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Opulenza.Application.Features.Users.Commands.UploadImage;

public class UploadUserImageCommand:IRequest<ErrorOr<UploadUserImageResult>>
{
    public IFormFile? File { get; set; } 
}