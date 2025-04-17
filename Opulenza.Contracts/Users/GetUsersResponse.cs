namespace Opulenza.Contracts.Users;

public class GetUsersResponse
{
    public List<GetUserResponse> Users { get; set; } = new();
    public required int TotalCount { get; set; }
}