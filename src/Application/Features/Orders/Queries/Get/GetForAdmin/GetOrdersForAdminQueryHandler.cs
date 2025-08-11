using Application.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries.Get.GetForAdmin;

internal class GetOrdersForAdminQueryHandler(IAppDbContext context)
    : IQueryHandler<GetOrdersForAdminQuery, List<GetOrdersForAdminQueryResponse>>
{
    private readonly IAppDbContext _context = context;

    public async Task<Result<List<GetOrdersForAdminQueryResponse>>> HandleAsync(GetOrdersForAdminQuery request, CancellationToken cancellationToken)
    {
        var orders = await _context.Orders.Select(o => new GetOrdersForAdminQueryResponse(
            o.Id,
            o.Status.GetName(),
            new(o.TotalAmount.Amount, o.TotalAmount.Currency),
            o.CustomerId,
            o.CreatedAt
        )).ToListAsync();

        return Result.Success(orders);
    }
}