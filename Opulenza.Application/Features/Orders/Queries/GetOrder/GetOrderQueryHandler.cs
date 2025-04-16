using ErrorOr;
using MediatR;
using Opulenza.Application.Common.interfaces;
using Opulenza.Application.Mapping;

namespace Opulenza.Application.Features.Orders.Queries.GetOrder;

public class GetOrderQueryHandler(IOrdersRepository ordersRepository): IRequestHandler<GetOrderQuery, ErrorOr<GetOrderResult>>
{
    public async Task<ErrorOr<GetOrderResult>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await  ordersRepository.GetOrderWithItemsAsync(request.Id!.Value, cancellationToken);
        if (order == null)
        {
            return Error.Validation("OrderNotFound", "order was not found.");
        }

        return order.MapToGetOrderResult();
    }
}