using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Authentication.Commands.ResetPassword;

public class ResetPasswordCommand: IRequest<ErrorOr<string>>
{
    public string Email { get; set; } = string.Empty;
    
    public string Token { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
}