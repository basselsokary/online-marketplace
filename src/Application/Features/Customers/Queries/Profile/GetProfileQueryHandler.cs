using Application.Common.Interfaces.Authentication;
using Application.DTOs;
using Domain.Errors;

namespace Application.Features.Customers.Queries.Profile;

internal class GetProfileQueryHandler(IIdentityService identityService,IUserContext userContext)
    : IQueryHandler<GetProfileQuery, CustomerDto>
{
    private readonly IIdentityService _identityService = identityService;
    private readonly IUserContext _userContext = userContext;

    public async Task<Result<CustomerDto>> HandleAsync(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var customerDto = await _identityService.GetCustomerDtoByIdAsync(_userContext.Id);

        if (customerDto == null)
            return Result.Failure<CustomerDto>(CustomerErrors.NotFound);

        return Result.Success(customerDto);
    }
}