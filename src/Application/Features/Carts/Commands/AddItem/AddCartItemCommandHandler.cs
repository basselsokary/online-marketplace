using Application.Common.Interfaces.Authentication;
using Domain.Entities;
using Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Carts.Commands.AddItem;

internal class AddCartItemCommandHandler(IAppDbContext context, IUserContext userContext)
    : ICommandHandler<AddCartItemCommand>
{
    private readonly IAppDbContext _context = context;
    private readonly IUserContext _userContext = userContext;

    public async Task<Result> HandleAsync(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        var productExist = await _context.Products.AnyAsync(p => p.Id == request.ProductId, cancellationToken);
        if (!productExist)
        {
            return Result.Failure(ProductErrors.NotFound(request.ProductId));
        }

        var cart = await _context.Carts.Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.CustomerId == _userContext.Id, cancellationToken);

        // If cart doesn't exist, create one
        if (cart == null)
        {
            cart = Cart.Create(_userContext.Id);
            await _context.Carts.AddAsync(cart, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        var result = cart.AddItem(request.ProductId, request.Quantity);
        if (!result.Succeeded)
            return result;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
