using FluentValidation;

namespace Application.Features.Products.Commands.DecreaseStock;

internal class DecreaseProductStockCommandValidator : AbstractValidator<DecreaseProductStockCommand>
{
    public DecreaseProductStockCommandValidator()
    {
        RuleFor(x => x.Products)
            .NotEmpty()
            .WithMessage("Products cannot be null or empty.");

        RuleForEach(x => x.Products).ChildRules(tuple =>
        {
            tuple.RuleFor(t => t.Id)
                .NotEmpty()
                .WithMessage("Product ID cannot be null or empty.");

            tuple.RuleFor(t => t.StockToBeDecreased)
                .GreaterThanOrEqualTo(1);
        });
    }   
}
