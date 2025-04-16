using ErrorOr;
using MediatR;
using Opulenza.Application.Features.Common;

namespace Opulenza.Application.Features.Orders.Queries.GetOrders;

public class GetOrdersQuery:PaginatedQuery, IRequest<ErrorOr<GetOrdersResult>>
{
}