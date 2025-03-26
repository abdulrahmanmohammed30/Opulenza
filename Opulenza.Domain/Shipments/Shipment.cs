using Opulenza.Domain.Users;

namespace Opulenza.Domain.Shipments;

public class Shipment
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public int OrderId { get; set; }
    
    public int UserAddressId { get; set; }

    // required at the database level 
    // could be nullable at the application level (not always included in the eager loading)
    public UserAddress? UserAddress { get; set; }
    
    public required string ShippingCompany { get; set; }
    public required string ShippingTracKId { get; set; }
    public required string ShippingTracKUrl { get; set; }

}