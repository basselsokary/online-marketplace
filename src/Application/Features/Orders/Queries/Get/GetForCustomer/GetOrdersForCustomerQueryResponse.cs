using Application.DTOs;
using Domain.Enums;

namespace Application.Features.Orders.Queries.Get.GetForCustomer;

public record GetOrdersForCustomerQueryResponse(
    Guid OrderId,
    OrderStatus Status,
    MoneyDto TotalAmount,
    DateTime CreatedAt
);