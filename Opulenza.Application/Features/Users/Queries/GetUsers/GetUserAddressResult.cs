namespace Opulenza.Application.Features.Users.Queries.GetUsers;

public class GetUserAddressResult
{
    public required string StreetAddress { get; set; }
    
    public required string Country { get; set; }
    
    public required string City { get; set; }
    
    public required string ZipCode { get; set; }
}