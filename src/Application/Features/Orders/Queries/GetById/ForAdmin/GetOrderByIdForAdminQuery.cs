namespace Application.Features.Orders.Queries.GetById.ForAdmin;

public record GetOrderByIdForAdminQuery(Guid Id) : IQuery<GetOrderByIdForAdminQueryResponse>;
