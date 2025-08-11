using Domain.Enums;

namespace Application.Features.Orders.Commands.UpdateStatus;

public record UpdateOrderStatusCommand(
    Guid Id,
    OrderStatus Status
) : ICommand;
