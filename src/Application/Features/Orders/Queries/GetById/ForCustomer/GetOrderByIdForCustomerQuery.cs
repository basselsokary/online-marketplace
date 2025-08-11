namespace Application.Features.Orders.Queries.GetById.ForCustomer;

public record GetOrderByIdForCustomerQuery(Guid Id) : IQuery<GetOrderByIdForCustomerQueryResponse>;
