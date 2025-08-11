using System;

namespace Application.Features.Reviews.Queries.GetById;

public record GetReviewByIdQuery(Guid Id) : IQuery<GetReviewByIdQueryResponse>;
