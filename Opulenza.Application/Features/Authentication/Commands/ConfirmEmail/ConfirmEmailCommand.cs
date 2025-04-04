using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Authentication.Commands.ConfirmEmail;

public class ConfirmEmailCommand: IRequest<ErrorOr<string>>
{
    public string? UserId { get; set; }
    public string? Token { get; set; }
}