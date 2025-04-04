using ErrorOr;
using MediatR;
using Opulenza.Application.Features.Authentication.Queries.Login;

namespace Opulenza.Application.Features.Authentication.LoginWithGitHubCallback;

public class LoginWithGitHubCallbackCommand: IRequest<ErrorOr<ExternalLoginResult>>
{
    public string ReturnUrl = "/";
    public string? RemoteError = null;
}