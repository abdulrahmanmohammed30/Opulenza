using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Authentication.Queries.Login;

public class LoginQuery: IRequest<ErrorOr<LoginResult>>
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}