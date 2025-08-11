using Application.Common.Interfaces.Authentication;
using Domain.Entities;
using Domain.Errors;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Commands.Place;

internal class PlaceOrderCommandHandler(IAppDbContext context, IUserContext userContext)
    : ICommandHandler<PlaceOrderCommand, Guid>
{
    private readonly IAppDbContext _context = context;
    private readonly IUserContext _userContext = userContext;

    public async Task<Result<Guid>> HandleAsync(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts.AsNoTracking().FirstOrDefaultAsync(c => c.CustomerId == _userContext.Id, cancellationToken);
        if (cart == null)
            return Result.Failure<Guid>(CartErrors.NotFound);
            
        // Validate all products and make sure they in stock
        var productIds = cart.CartItems.Select(item => item.ProductId).ToList();
        var products = await _context.Products
            .AsNoTracking()
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync(cancellationToken);

        foreach (var item in cart.CartItems)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);

            if (product == null)
                return Result.Failure<Guid>(ProductErrors.NotFound(item.ProductId));

            if (product.UnitsInStock < item.Quantity)
                return Result.Failure<Guid>(ProductErrors.InsufficientStock(item.ProductId, product.UnitsInStock));
        }

        // Create order
        var address = Address.Create(
            request.Address.Street,
            request.Address.District,
            request.Address.City,
            request.Address.ZipCode);

        var order = Order.Create(_userContext.Id, address);
        decimal totalAmount = 0.0m;

        // Add order items
        foreach (var item in cart.CartItems)
        {
            var product = products.First(p => p.Id == item.ProductId);
            var result = order.AddOrderItem(item.ProductId, item.Quantity, product.Price);
            if (!result.Succeeded)
                return result.To<Guid>();

            totalAmount += product.Price.Amount;
        }

        order.UpdateTotalAmount(totalAmount);

        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(order.Id);
    }
}