using Application.DTOs;
using Domain.Enums;

namespace Application.Features.Orders.Commands.Place;

public record PlaceOrderCommand(
    AddressDto Address,
    PaymentMethod PaymentMethod
) : ICommand<Guid>;




