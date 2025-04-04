namespace Opulenza.Application.Features.Users.Queries.GetUser;

public class GetUserAddressQueryResult 
{
    public required string StreetAddress { get; set; }
    
    public required string Country { get; set; }
    
    public required string City { get; set; }
    
    public required string ZipCode { get; set; }
}