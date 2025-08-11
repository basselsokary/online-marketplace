namespace Application.Features.Reviews.Queries.GetById;

public record GetReviewByIdQueryResponse(
    Guid Id,
    Guid ProductId,
    int Rating,
    string? Comment
);