using Application.DTOs;

namespace Application.Features.Customers.Queries.Profile;

public record GetProfileQuery() : IQuery<CustomerDto>;
