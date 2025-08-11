
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Commands.DecreaseStock;

internal class DecreaseProductStockCommandHandler(IAppDbContext context)
    : ICommandHandler<DecreaseProductStockCommand>
{
    private readonly IAppDbContext _context = context;

    public async Task<Result> HandleAsync(DecreaseProductStockCommand request, CancellationToken cancellationToken)
    {
        var productsIds = request.Products.Select(x => x.Id);
        var products = await _context.Products
            .AsTracking()
            .Where(p => productsIds.Contains(p.Id))
            .ToListAsync(cancellationToken);

        foreach (var productEntity in products)
        {
            var tuple = request.Products.First(t => t.Id == productEntity.Id);
            var result = productEntity.Purchsased(tuple.StockToBeDecreased);
            if (!result.Succeeded)
                return result;
        }

        return Result.Success();
    }
}
