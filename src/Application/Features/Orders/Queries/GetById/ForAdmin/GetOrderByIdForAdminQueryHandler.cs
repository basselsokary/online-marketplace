using System.Linq.Expressions;
using Application.Common.Interfaces.Authentication;
using Application.DTOs;
using Domain.Entities;
using Domain.Errors;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries.GetById.ForAdmin;

internal class GetOrderByIdForAdminQueryHandler(IAppDbContext context, IIdentityService identityService)
    : IQueryHandler<GetOrderByIdForAdminQuery, GetOrderByIdForAdminQueryResponse>
{
    private readonly IAppDbContext _context = context;
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result<GetOrderByIdForAdminQueryResponse>> HandleAsync(GetOrderByIdForAdminQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.Id == request.Id)
            .Select(order => new
            {
                order.Id,
                order.CreatedAt,
                order.DeliveredAt,
                PaidAt = (DateTime?)null,
                order.ShippedAt,
                order.Status,
                order.CustomerId,
                Address = new AddressDto(order.Address.Street, order.Address.District, order.Address.City, order.Address.ZipCode),
                TotalAmount = new MoneyDto(order.TotalAmount.Amount, order.TotalAmount.Currency),
                OrderItemDtos = order.OrderItems.Select(item => new OrderItemDto(
                    item.Id,
                    item.ProductId,
                    item.Quantity,
                    new(item.UnitPrice.Amount, item.UnitPrice.Currency)
                )).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (order == null)
        {
            return Result.Failure<GetOrderByIdForAdminQueryResponse>(OrderErrors.NotFound(request.Id));
        }

        var customer = await _identityService.GetCustomerDtoByIdAsync(order.CustomerId)
            ?? throw new ApplicationException("Customer doesn't exist!!");

        var response = new GetOrderByIdForAdminQueryResponse(
            order.Id,
            order.CreatedAt,
            order.DeliveredAt,
            order.PaidAt,
            order.ShippedAt,
            order.Status,
            order.CustomerId,
            customer.FullName,
            customer.Email,
            customer.PhoneNumber,
            order.Address,
            order.TotalAmount,
            order.OrderItemDtos
        );

        return Result.Success(response);
    }
}