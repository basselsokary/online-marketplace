using FluentValidation;

namespace Application.Features.Orders.Queries.GetById.ForCustomer;

internal class GetOrderByIdForCustomerQueryValidator : AbstractValidator<GetOrderByIdForCustomerQuery>
{
    public GetOrderByIdForCustomerQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Order ID is required.");
    }
}