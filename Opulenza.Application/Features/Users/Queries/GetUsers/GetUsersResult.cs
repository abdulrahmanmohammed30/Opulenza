namespace Opulenza.Application.Features.Users.Queries.GetUsers;

public class GetUsersResult
{
    public List<GetUserResult> Users { get; set; } = new();
    public required int TotalCount { get; set; }
}