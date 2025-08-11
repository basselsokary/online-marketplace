namespace Application.Features.Products.Queries.Get;

public record GetProductsQuery(string? Search, Guid CategoryId) : IQuery<List<GetProductsQueryResponse>>;
