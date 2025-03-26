using Opulenza.Domain.Common;

namespace Opulenza.Domain.Entities.Users;

public class UserAddress: Entity
{
    public int UserId { get; set; }
    public required string StreetAddress { get; set; }
    
    public required string Country { get; set; }
    
    public required string City { get; set; }
    
    public required string ZipCode { get; set; }
}

