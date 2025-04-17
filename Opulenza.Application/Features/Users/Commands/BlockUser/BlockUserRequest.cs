namespace Opulenza.Application.Features.Users.Commands.BlockUser;

public class BlockUserRequest
{
    public DateTime? BlockedUntil { get; set; }
    public string? Reason { get; set; }
}