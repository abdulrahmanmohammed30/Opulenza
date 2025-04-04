using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Users.Commands.Update;

public class UpdateUserCommand: IRequest<ErrorOr<string>>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Username { get; set; }
}