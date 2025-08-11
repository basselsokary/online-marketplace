using FluentValidation;

namespace Application.Features.Orders.Queries.GetById.ForAdmin;

internal class GetOrderByIdForAdminQueryValidator : AbstractValidator<GetOrderByIdForAdminQuery>
{
    public GetOrderByIdForAdminQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Order ID is required.");
    }
}