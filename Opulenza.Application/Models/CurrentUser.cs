namespace Opulenza.Application.Models;

public class CurrentUser
{
    public required int Id { get; init; }
    public required string Username { get; init; }
    
    public required string Role { get; init; }
    
    public required string Email { get; set; }
}