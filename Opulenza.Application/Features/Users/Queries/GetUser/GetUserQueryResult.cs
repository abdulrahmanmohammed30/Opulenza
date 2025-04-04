namespace Opulenza.Application.Features.Users.Queries.GetUser;

public class GetUserQueryResult
{
    public required string Username { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    
    public GetUserAddressQueryResult? Address { get; set; }
    public required string? ImageUrl { get; set; }
}