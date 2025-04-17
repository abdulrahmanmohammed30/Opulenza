namespace Opulenza.Contracts.Users;

public class GetUserResponse
{
    public required int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required DateTime Joined { get; set; }
    public string? ImageUrl { get; set; }
    public GetUserAddressResponse? Address { get; set; }
}