namespace Opulenza.Contracts.Users;

public class UserResponse
{
    public required string Username { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    
    public UserAddressResponse? Address { get; set; }
    public required string? ImageUrl { get; set; }
}