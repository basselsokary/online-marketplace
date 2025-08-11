using Application.Common.Interfaces.Authentication;
using Domain.Errors;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Customers.Commands.Update;

internal class UpdateCustomerCommandHandler(IIdentityService identityService, IUserContext userContext)
    : ICommandHandler<UpdateCustomerCommand>
{
    private readonly IIdentityService _identityService = identityService;
    private readonly IUserContext _userContext = userContext;

    public async Task<Result> HandleAsync(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var address = Address.Create(
            request.Address.Street,
            request.Address.District,
            request.Address.City,
            request.Address.ZipCode);

        var result = await _identityService.UpdateCustomerAsync(
            _userContext.Id,
            request.FullName,
            address,
            request.PhoneNumber,
            request.Age,
            cancellationToken);
        return result;
    }
}