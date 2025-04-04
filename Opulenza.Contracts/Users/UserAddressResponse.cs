namespace Opulenza.Contracts.Users;

public class UserAddressResponse
{
    public required string StreetAddress { get; set; }

    public required string Country { get; set; }

    public required string City { get; set; }

    public required string ZipCode { get; set; }
}
