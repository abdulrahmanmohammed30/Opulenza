using Opulenza.Contracts.Common;

namespace Opulenza.Contracts.Users;

public class GetUsersRequest: PaginatedRequest 
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public DateTime? Joined { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Sort { get; set; }
}
