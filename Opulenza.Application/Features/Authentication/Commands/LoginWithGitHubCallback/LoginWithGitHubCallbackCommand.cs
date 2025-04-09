using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Authentication.Commands.LoginWithGitHubCallback;

public class LoginWithGitHubCallbackCommand: IRequest<ErrorOr<ExternalLoginResult>>
{
    public string ReturnUrl = "/";
    public string? RemoteError = null;
}