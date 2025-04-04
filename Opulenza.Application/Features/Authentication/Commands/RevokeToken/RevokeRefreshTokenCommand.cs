using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Authentication.Commands.RevokeToken;

public class RevokeRefreshTokenCommand:IRequest<ErrorOr<string>>
{
}