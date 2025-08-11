using Application.Common.Interfaces.Authentication;
using Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Carts.Commands.Clear;

internal class ClearCartCommandHandler(IAppDbContext context, IUserContext userContext)
    : ICommandHandler<ClearCartCommand>
{
    private readonly IAppDbContext _context = context;
    private readonly IUserContext _userContext = userContext;

    public async Task<Result> HandleAsync(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.CustomerId == _userContext.Id, cancellationToken);

        if (cart == null)
        {
            return Result.Failure(CartErrors.NotFound);
        }

        cart.Clear();
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
