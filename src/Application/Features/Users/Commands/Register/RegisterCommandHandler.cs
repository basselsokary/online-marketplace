using Application.Common.Interfaces.Authentication;
using Domain.Enums;
using Domain.Errors;

namespace Application.Features.Users.Commands.Register;

internal class RegisterCommandHandler(IIdentityService userService) : ICommandHandler<RegisterCommand>
{
    private readonly IIdentityService _userService = userService;

    public async Task<Result> HandleAsync(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.CheckUserExistsAsync(request.UserName, request.Email);
        if (!result.Succeeded)
            return result;

        var createdResult = await _userService.CreateCustomerAsync(request);
            
        if (!createdResult.Result.Succeeded)
            return createdResult.Result;

        return Result.Success();
    }
}
