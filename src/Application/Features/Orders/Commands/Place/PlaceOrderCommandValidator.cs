using Application.Common.Validators;
using FluentValidation;

namespace Application.Features.Orders.Commands.Place;

internal class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
{
    public PlaceOrderCommandValidator()
    {
        RuleFor(x => x.PaymentMethod)
            .IsInEnum()
            .WithMessage("Invalid payment method.");

        RuleFor(x => x.Address)
            .NotNull()
            .WithMessage("Address details are required.")
            .SetValidator(new AddressDtoValidator())
            .WithMessage("Invalid address details.");
    }
}

