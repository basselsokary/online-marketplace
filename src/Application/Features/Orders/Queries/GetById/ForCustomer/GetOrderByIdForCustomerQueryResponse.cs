using Application.DTOs;
using Domain.Enums;

namespace Application.Features.Orders.Queries.GetById.ForCustomer;

public record GetOrderByIdForCustomerQueryResponse(
    Guid Id,
    DateTime CreatedAt,
    DateTime? DeliveredAt,
    DateTime? PaidAt,
    DateTime? ShippedAt,
    OrderStatus Status,
    string CustomerId,
    AddressDto Address,
    MoneyDto TotalAmount,
    IReadOnlyCollection<OrderItemDto> OrderItems
);
