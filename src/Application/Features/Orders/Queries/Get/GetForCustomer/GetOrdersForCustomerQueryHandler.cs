using System.Linq.Expressions;
using Application.Common.Interfaces.Authentication;
using Application.Helpers;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries.Get.GetForCustomer;

internal class GetOrdersForCustomerQueryHandler(IAppDbContext context, IUserContext userContext)
    : IQueryHandler<GetOrdersForCustomerQuery, List<GetOrdersForCustomerQueryResponse>>
{
    private readonly IAppDbContext _context = context;
    private readonly IUserContext _userContext = userContext;

    public async Task<Result<List<GetOrdersForCustomerQueryResponse>>> HandleAsync(
        GetOrdersForCustomerQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await _context.Orders
            .Where(o => o.CustomerId == _userContext.Id)
            .Select(GetProjection())
            .ToListAsync(cancellationToken);

        return Result.Success(orders);
    }

    private static Expression<Func<Order, GetOrdersForCustomerQueryResponse>> GetProjection()
    {
        return order => new GetOrdersForCustomerQueryResponse(
            order.Id,
            order.Status,
            new(order.TotalAmount.Amount, order.TotalAmount.Currency),
            order.CreatedAt
        );
    }
}