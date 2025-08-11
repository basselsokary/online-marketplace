using Application.Common.Interfaces.Authentication;
using Application.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Carts.Queries.Get;

internal class GetCartItemsQueryHandler(IAppDbContext context, IUserContext userContext)
    : IQueryHandler<GetCartItemsQuery, GetCartItemsQueryResponse>
{
    private readonly IAppDbContext _context = context;
    private readonly IUserContext _userContext = userContext;

    public async Task<Result<GetCartItemsQueryResponse>> HandleAsync(GetCartItemsQuery request, CancellationToken cancellationToken)
    {
        var cartId = await _context.Carts
            .Where(c => c.CustomerId == _userContext.Id)
            .Select(c => c.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (cartId == default)
            return Result.Success<GetCartItemsQueryResponse>();

        var cartItems = await _context.CartItems
            .Where(ci => ci.CartId == cartId)
            .Select(ci => new
            {
                ci.Id,
                ci.ProductId,
                ci.Quantity
            }).ToListAsync(cancellationToken);

        if (cartItems.Count == 0)
            return Result.Success<GetCartItemsQueryResponse>();
        
        var productIds = cartItems.Select(ci => ci.ProductId).ToList();

        var products = await _context.Products.Where(p => productIds.Contains(p.Id))
            .AsNoTracking()
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.ImageURL
            }).ToListAsync(cancellationToken);

        if (products.Count != productIds.Count)
            return Result.Failure<GetCartItemsQueryResponse>(["Some products were not found."]);

        decimal totalPrice = 0.0m;
        var productMap = products.ToDictionary(p => p.Id);

        var cartItemsRecord = cartItems.Select(ci =>
        {
            var product = productMap[ci.ProductId];
            totalPrice += ci.Quantity * product.Price.Amount;
            return new CartItem(
                ci.Id,
                ci.ProductId,
                ci.Quantity,
                product.Name,
                product.Description,
                product.ImageURL,
                new(product.Price.Amount, product.Price.Currency)
            );
        }).ToList();

        GetCartItemsQueryResponse response = new(cartItemsRecord, totalPrice);

        return Result.Success(response);
    }
}