using Application.DTOs;

namespace Application.Features.Customers.Queries.GetAll;

public record GetAllCustomersQuery() : IQuery<List<CustomerDto>>;
