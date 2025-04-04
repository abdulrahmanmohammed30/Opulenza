using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Authentication.Commands.RequestResetPassword;

public class RequestResetPasswordCommand: IRequest<ErrorOr<string>>
{
    public string? Email { get; set; }
}