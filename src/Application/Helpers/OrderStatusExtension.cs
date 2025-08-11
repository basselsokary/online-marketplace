using Domain.Enums;

namespace Application.Helpers;

public static class OrderStatusExtension
{
    public static string GetName(this OrderStatus orderStatus) => orderStatus switch
    {
        OrderStatus.Pending => nameof(OrderStatus.Pending),
        OrderStatus.Confirmed => nameof(OrderStatus.Confirmed),
        OrderStatus.Shipped => nameof(OrderStatus.Shipped),
        OrderStatus.Delivered => nameof(OrderStatus.Delivered),
        OrderStatus.Cancelled => nameof(OrderStatus.Cancelled),
        _ => throw new ArgumentOutOfRangeException(nameof(orderStatus), orderStatus, null)
    };

    public static OrderStatus GetOrderStatus(this string orderStatusString) => orderStatusString switch
    {
        nameof(OrderStatus.Pending) => OrderStatus.Pending,
        nameof(OrderStatus.Confirmed) => OrderStatus.Confirmed,
        nameof(OrderStatus.Shipped) => OrderStatus.Shipped,
        nameof(OrderStatus.Delivered) => OrderStatus.Delivered,
        nameof(OrderStatus.Cancelled) => OrderStatus.Cancelled,
        _ => throw new ArgumentOutOfRangeException(orderStatusString, orderStatusString, null)
    };
}
