using ErrorOr;
using MediatR;
using Opulenza.Application.Features.Common;

namespace Opulenza.Application.Features.Users.Queries.GetUsers;

public class GetUsersQuery: PaginatedQuery, IRequest<ErrorOr<GetUsersResult>>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public DateTime? Joined { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public GetUsersSortBy SortBy { get; set; }
    public SortOptions SortOptions { get; set; }
}
