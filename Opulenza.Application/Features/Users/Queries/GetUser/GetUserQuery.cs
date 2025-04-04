using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Users.Queries.GetUser;

public class GetUserQuery: IRequest<ErrorOr<GetUserQueryResult>>
{
    
}