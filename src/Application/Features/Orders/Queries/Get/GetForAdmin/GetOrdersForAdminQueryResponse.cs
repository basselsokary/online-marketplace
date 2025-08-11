using Application.DTOs;

namespace Application.Features.Orders.Queries.Get.GetForAdmin;

public record GetOrdersForAdminQueryResponse(
    Guid Id,
    string Status,
    MoneyDto TotalAmount,
    string CustomerId,
    DateTime CreatedAt
);