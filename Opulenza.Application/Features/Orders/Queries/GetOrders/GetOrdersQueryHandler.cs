using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Mapping;

namespace Opulenza.Application.Features.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler(IOrdersRepository ordersRepository):IRequestHandler<GetOrdersQuery, ErrorOr<GetOrdersResult>>
{
    public async Task<ErrorOr<GetOrdersResult>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await ordersRepository.GetOrdersAsync(request, cancellationToken);
        return orders.MapToGetOrdersResult();
    }
}