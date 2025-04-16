using FluentValidation;
using Opulenza.Application.Features.Common;

namespace Opulenza.Application.Features.Orders.Queries.GetOrders;

public class GetOrdersQueryValidator: AbstractValidator<GetOrdersQuery>
{
    public GetOrdersQueryValidator()
    {
        Include(new PaginatedQueryValidator());
    }
}