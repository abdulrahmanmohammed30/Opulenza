using ErrorOr;
using MediatR;
using Opulenza.Application.Features.Authentication.Queries.Login;

namespace Opulenza.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommand: IRequest<ErrorOr<TokenResult>>
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
}