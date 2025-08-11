using Application.Common.Interfaces.Authentication;
using Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Reviews.Queries.GetById;

internal class GetReviewByIdQueryHandler(IAppDbContext context, IUserContext userContext)
    : IQueryHandler<GetReviewByIdQuery, GetReviewByIdQueryResponse>
{
    private readonly IAppDbContext _context = context;
    private readonly IUserContext _userContext = userContext;

    public async Task<Result<GetReviewByIdQueryResponse>> HandleAsync(GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        var review = await _context.Reviews
            .Where(r => r.Id == request.Id && r.CustomerId.ToString() == _userContext.Id)
            .Select(r => new GetReviewByIdQueryResponse(
                r.Id,
                r.ProductId,
                r.Rating,
                r.Comment
            )).FirstOrDefaultAsync(cancellationToken);

        if (review == null)
        {
            return Result.Failure<GetReviewByIdQueryResponse>(ReviewErrors.NotFound(request.Id));
        }

        return Result.Success(review);
    }
}
