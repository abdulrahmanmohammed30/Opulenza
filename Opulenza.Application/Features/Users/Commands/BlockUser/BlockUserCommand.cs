using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Users.Commands.BlockUser;

public class BlockUserCommand: IRequest<ErrorOr<string>>
{
    public int? UserId { get; set; }
    public DateTime? BlockedUntil { get; set; }
    public string? Reason { get; set; }
}