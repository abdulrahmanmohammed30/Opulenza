namespace Opulenza.Contracts.Users;

public class UpdateUserAddressRequest
{
    public string? StreetAddress { get; init; }
    
    public string? Country { get; init; }
    
    public string? City { get; init; }
    
    public  string? ZipCode { get; init; }
}