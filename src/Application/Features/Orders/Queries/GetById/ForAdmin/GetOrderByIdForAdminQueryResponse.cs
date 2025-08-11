using Application.DTOs;
using Domain.Enums;

namespace Application.Features.Orders.Queries.GetById.ForAdmin;

public record GetOrderByIdForAdminQueryResponse(
    Guid Id,
    DateTime CreatedAt,
    DateTime? DeliveredAt,
    DateTime? PaidAt,
    DateTime? ShippedAt,
    OrderStatus Status,
    string CustomerId,
    string CustomerName,
    string CustomerEmail,
    string CustomerPhoneNumber,
    AddressDto Address,
    MoneyDto TotalAmount,
    IReadOnlyCollection<OrderItemDto> OrderItems
);
