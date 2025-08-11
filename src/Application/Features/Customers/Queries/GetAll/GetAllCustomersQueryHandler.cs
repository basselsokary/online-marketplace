using Application.Common.Interfaces.Authentication;
using Application.DTOs;

namespace Application.Features.Customers.Queries.GetAll;

internal class GetAllCustomersQueryHandler(IIdentityService identityService)
    : IQueryHandler<GetAllCustomersQuery, List<CustomerDto>>
{
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result<List<CustomerDto>>> HandleAsync(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _identityService.GetAllCustomersAsync();

        return Result.Success(customers);
    }
}
