using System.Linq.Expressions;
using Domain.Entities;
using Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries.Get;

internal class GetProductsQueryHandler(IAppDbContext context)
    : IQueryHandler<GetProductsQuery, List<GetProductsQueryResponse>>
{
    private readonly IAppDbContext _context = context;

    public async Task<Result<List<GetProductsQueryResponse>>> HandleAsync(GetProductsQuery request, CancellationToken cancellationToken)
    {
        List<GetProductsQueryResponse> products;
        IQueryable<Product> query = _context.Products.AsNoTracking().Include(p => p.CategoryProducts);
        if (request.CategoryId != default)
        {
            var isCategoryExist = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
            if (!isCategoryExist)
            {
                return Result.Failure<List<GetProductsQueryResponse>>(CategoryErrors.NotFound(request.CategoryId));
            }

            query = query.Where(p => p.CategoryProducts.Any(cp => cp.CategoryId == request.CategoryId));
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(p => p.Name.Contains(request.Search));
        }

        products = await query
            .Select(GetProjection())
            .ToListAsync(cancellationToken);

        return Result.Success(products);
    }

    private Expression<Func<Product, GetProductsQueryResponse>> GetProjection()
    {
        return product => new GetProductsQueryResponse(
            product.Id,
            product.Name,
            product.Description,
            product.UnitsInStock,
            new(product.Price.Amount, product.Price.Currency),
            product.ImageURL,
            product.CategoryProducts.Select(cp => cp.CategoryName).ToList()
        );
    }
}