using Domain.Entities;
using Domain.Enums;
using Domain.Errors;

namespace Application.Features.Orders.Commands.UpdateStatus;

internal class UpdateOrderStatusCommandHandler(IAppDbContext context)
    : ICommandHandler<UpdateOrderStatusCommand>
{
    private readonly IAppDbContext _context = context;

    public async Task<Result> HandleAsync(UpdateOrderStatusCommand command, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .FindAsync([command.Id], cancellationToken);

        if (order is null)
        {
            return Result.Failure(OrderErrors.NotFound(command.Id));
        }

        var result = SetStatus(command.Status, order);
        if (!result.Succeeded)
            return result;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static Result SetStatus(OrderStatus status, Order order) => status switch
    {
        OrderStatus.Pending => Result.Failure(["Status can't be pending."]),
        OrderStatus.Confirmed => order.Confirm(),
        OrderStatus.Shipped => order.Ship(),
        OrderStatus.Delivered => order.Deliver(),
        OrderStatus.Cancelled => order.Cancel(),
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };
}
