using System.Linq.Expressions;
using Application.Common.Interfaces.Authentication;
using Application.DTOs;
using Domain.Entities;
using Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries.GetById.ForCustomer;

internal class GetOrderByIdForCustomerQueryHandler(IAppDbContext context, IUserContext userContext)
    : IQueryHandler<GetOrderByIdForCustomerQuery, GetOrderByIdForCustomerQueryResponse>
{
    private readonly IAppDbContext _context = context;
    private readonly IUserContext _userContext = userContext;

    public async Task<Result<GetOrderByIdForCustomerQueryResponse>> HandleAsync(GetOrderByIdForCustomerQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.Id == request.Id && o.CustomerId == _userContext.Id)
            .Select(GetProjection())
            .FirstOrDefaultAsync(cancellationToken);

        if (order == null)
        {
            return Result.Failure<GetOrderByIdForCustomerQueryResponse>(OrderErrors.NotFound(request.Id));
        }

        return Result.Success(order);
    }

    private static Expression<Func<Order, GetOrderByIdForCustomerQueryResponse>> GetProjection()
    {
        return order => new(
            order.Id,
            order.CreatedAt,
            order.DeliveredAt,
            null,
            order.ShippedAt,
            order.Status,
            order.CustomerId,
            new(order.Address.Street, order.Address.District, order.Address.City, order.Address.ZipCode),
            new(order.TotalAmount.Amount, order.TotalAmount.Currency),
            order.OrderItems.Select(item => new OrderItemDto(
                item.Id,
                item.ProductId,
                item.Quantity,
                new(item.UnitPrice.Amount, item.UnitPrice.Currency)
            )).ToList()
        );
    }
}