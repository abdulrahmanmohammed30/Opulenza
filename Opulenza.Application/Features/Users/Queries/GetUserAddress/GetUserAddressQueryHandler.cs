using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Features.Users.Queries.GetUser;
using Opulenza.Application.Mapping;

namespace Opulenza.Application.Features.Users.Queries.GetUserAddress;

public class GetUserAddressQueryHandler(ICurrentUserProvider currentUserProvider,
    IUserAddressRepository userAddressRepository)
    : IRequestHandler<GetUserAddressQuery, ErrorOr<GetUserAddressQueryResult>>
{
    public async Task<ErrorOr<GetUserAddressQueryResult>> Handle(GetUserAddressQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        var userAddress = await userAddressRepository.GetByUserId(userId, cancellationToken);

        if (userAddress == null)
        {
            return Error.NotFound();
        }

        return userAddress.MapToGetAddressUserResult();
    }
}