namespace Domain.Errors;

public static class OrderErrors
{
    public static IEnumerable<string> NotFound(Guid id)
        => [
        "Order.NotFound",
        $"Order {id} not found."
    ];

    public static IEnumerable<string> CantBeCancelled(Guid id)
        => [
        "Order.CantBeCancelled",
        $"Order {id} cannot be cancelled unless it is pending."
    ];

    public static IEnumerable<string> CantBeConfirmed(Guid id)
        => [
        "Order.CantBeConfirmed",
        $"Order {id} cannot be confirmed unless it is pending."
    ];

    public static IEnumerable<string> AlreadyDelivered(Guid id)
        => [
        "Order.AlreadyDelivered",
        $"Order {id} is already delivered."
    ];
    
    public static IEnumerable<string> CantBeShipped(Guid id)
        => [
        "Order.CantBeShipped",
        $"Order {id} is either not confirmed or already delivered."
    ];

    public static IEnumerable<string> ItemAlreadyExists(Guid productId)
        => [
        "Order.ItemAlreadyExists",
        $"Order item of product {productId} already exists in the order."
    ];

    public static IEnumerable<string> InvalidQuantity(int quantity)
        => [
        "Order.InvalidQuantity",
        $"Invalid quantity: {quantity}. Quantity must be greater than zero."
    ];
}