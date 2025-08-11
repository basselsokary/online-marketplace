namespace Application.Features.Users.Queries.GetUser.ById;

public record GetUserByIdQuery(string? UserId = null) : IQuery<GetUserByIdQueryResponse>;
