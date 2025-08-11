using Domain.Common;
using Domain.Enums;
using Domain.Errors;
using Domain.ValueObjects;
using SharedKernel.Models;

namespace Domain.Entities;

public class Order : BaseAuditableEntity<Guid>
{
    private Order() : base(Guid.Empty) { }
    private Order(string customerId, Address address) : base(Guid.NewGuid())
    {
        CustomerId = customerId;
        Address = address;

        Status = OrderStatus.Pending;
        DeliveredAt = null;
    }

    public DateTime? DeliveredAt { get; private set; }

    public DateTime? ShippedAt { get; private set; }

    public DateTime? CancelledAt { get; private set; }

    public OrderStatus Status { get; private set; }

    public string CustomerId { get; private set; } = null!;

    public Address Address { get; private set; } = null!;

    public Money TotalAmount { get; private set; } = Money.Empty;

    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public static Order Create(
        string customerId,
        Address address)
    {
        return new Order(customerId, address);
    }

    public Result AddOrderItem(Guid productId, int quantity, Money price)
    {
        if (_orderItems.Any(x => x.ProductId == productId))
        {
            return Result.Failure(OrderErrors.ItemAlreadyExists(productId));
        }

        var orderItem = OrderItem.Create(productId, quantity, price);
        _orderItems.Add(orderItem);

        UpdateTotalAmount(orderItem.UnitPrice);

        return Result.Success();
    }

    public void UpdateTotalAmount(decimal totalAmount)
    {
        TotalAmount = Money.Create(totalAmount);
    }

    public Result Cancel()
    {
        if (Status != OrderStatus.Pending)
        {
            return Result.Failure(OrderErrors.CantBeCancelled(Id));
        }

        Status = OrderStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result Confirm()
    {
        if (Status != OrderStatus.Pending)
        {
            return Result.Failure(OrderErrors.CantBeConfirmed(Id));
        }

        Status = OrderStatus.Confirmed;

        return Result.Success();
    }

    public Result Deliver()
    {
        if (Status == OrderStatus.Delivered)
        {
            return Result.Failure(OrderErrors.AlreadyDelivered(Id));
        }

        Status = OrderStatus.Delivered;
        DeliveredAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result Ship()
    {
        if (Status != OrderStatus.Confirmed)
        {
            return Result.Failure(OrderErrors.CantBeShipped(Id));
        }

        Status = OrderStatus.Shipped;
        DeliveredAt = DateTime.UtcNow;

        return Result.Success();
    }

    private void UpdateTotalAmount(Money totalPrice)
    {
        if (TotalAmount == Money.Empty)
        {
            TotalAmount = totalPrice;
            return;
        }

        TotalAmount.Add(totalPrice);
    }
}
