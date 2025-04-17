using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;

namespace Opulenza.Application.Features.Users.Queries.GetUsers;

public class GetUsersQueryHandler(IUserRepository userRepository): IRequestHandler<GetUsersQuery, ErrorOr<GetUsersResult>>
{
    public async Task<ErrorOr<GetUsersResult>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var getUsersResult = await userRepository.GetUsersAsync(request, cancellationToken);
        return getUsersResult;
    }
}