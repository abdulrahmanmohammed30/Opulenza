using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<ErrorOr<string>>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}