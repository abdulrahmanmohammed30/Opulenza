namespace Opulenza.Contracts.Orders;

public class GetOrdersResponse
{
    public List<GetSingleOrderResponse> Orders { get; set; }
}