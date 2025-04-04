using Opulenza.Application.Features.Authentication.Queries.Login;

namespace Opulenza.Application.Features.Authentication.LoginWithGitHubCallback;

public class ExternalLoginResult: LoginResult
{
    public required string ReturnUrl { get; set; }
}