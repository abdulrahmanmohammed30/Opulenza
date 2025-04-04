using ErrorOr;
using MediatR;
using Opulenza.Application.Features.Users.Queries.GetUser;

namespace Opulenza.Application.Features.Users.Queries.GetUserAddress;

public class GetUserAddressQuery: IRequest<ErrorOr<GetUserAddressQueryResult>>
{
    
}