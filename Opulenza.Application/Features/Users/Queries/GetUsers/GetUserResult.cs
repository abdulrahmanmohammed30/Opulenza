namespace Opulenza.Application.Features.Users.Queries.GetUsers;

public class GetUserResult
{
    public required int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required DateTime Joined { get; set; }
    public string? ImageUrl { get; set; }
    public GetUserAddressResult? Address { get; set; }
}