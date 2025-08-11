using Application.Common.Interfaces.Authentication;
using Application.DTOs;
using Domain.Errors;

namespace Application.Features.Users.Queries.GetUser.ByEmail;

internal class GetUserByEmailQueryHandler(IIdentityService identityService)
    : IQueryHandler<GetUserByEmailQuery, GetUserByEmailQueryResponse>
{
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result<GetUserByEmailQueryResponse>> HandleAsync(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
       var user = await _identityService.GetUserDtoByEmailAsync(request.Email);

       if (user is null)
           return Result.Failure<GetUserByEmailQueryResponse>(UserErrors.NotFound);

       return Result.Success(ToResponse(user));
    }

    private static GetUserByEmailQueryResponse? ToResponse(UserDto user)
    {
        return new(user.Id, user.UserName, user.Email, user.Roles);
    }
}