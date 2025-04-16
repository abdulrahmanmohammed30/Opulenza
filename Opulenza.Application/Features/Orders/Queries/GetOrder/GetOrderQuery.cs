using ErrorOr;
using MediatR;

namespace Opulenza.Application.Features.Orders.Queries.GetOrder;

public class GetOrderQuery: IRequest<ErrorOr<GetOrderResult>>
{
    public int? Id { get; set; }
}