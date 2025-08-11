using Application.DTOs;

namespace Application.Features.Carts.Queries.Get;

public record GetCartItemsQueryResponse(
    List<CartItem> CartItems,
    decimal TotalPrice
);

public record CartItem(
    Guid Id,
    Guid ProductId,
    int Quantity,
    string ProductName,
    string? ProductDescription,
    string? ProductImageUrl,
    MoneyDto ProductPrice
);