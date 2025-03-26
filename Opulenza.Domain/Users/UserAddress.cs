using Opulenza.Domain.Identity;

namespace Opulenza.Domain.Users;

public abstract class UserAddress
{
    public int Id { get; set; }
    
    public string StreetAddress { get; set; } = null!;
    
    public string Country { get; set; }= null!;
    
    public string City { get; set; }= null!;
    
    public string ZipCode { get; set; }= null!;
    
    public int UserId { get; set; }
}

