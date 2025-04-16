using Opulenza.Domain.Entities.Enums;

namespace Opulenza.Application.Features.Orders.Queries.GetOrders;

public class GetOrdersResult
{
   public List<GetSingleOrderResult> Orders { get; set; }
}  
