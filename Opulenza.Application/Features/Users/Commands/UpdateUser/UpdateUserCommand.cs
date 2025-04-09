using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommand: IRequest<ErrorOr<string>>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}