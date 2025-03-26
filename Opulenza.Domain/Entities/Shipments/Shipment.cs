using Microsoft.EntityFrameworkCore;
using Opulenza.Domain.Common;
using Opulenza.Domain.Entities.Orders;
using Opulenza.Domain.Entities.Users;

namespace Opulenza.Domain.Entities.Shipments;

public class Shipment: Entity
{
    public int OrderId { get; set; }
    
    public Order? Order { get; set; }
    
    public int? UserAddressId { get; set; }

    // required at the database level 
    // could be nullable at the application level (not always included in the eager loading)
    public UserAddress? UserAddress { get; set; }
    
    public required string ShippingCompany { get; set; }
    public required string ShippingTracKId { get; set; }
    public required string ShippingTracKUrl { get; set; }

}