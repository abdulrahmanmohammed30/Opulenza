using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Users.Commands.ChangeUserPassword;

public class ChangeUserPasswordCommand: IRequest<ErrorOr<string>>
{
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
}