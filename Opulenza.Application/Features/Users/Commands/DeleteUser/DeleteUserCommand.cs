using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommand: IRequest<ErrorOr<string>>
{
    
}