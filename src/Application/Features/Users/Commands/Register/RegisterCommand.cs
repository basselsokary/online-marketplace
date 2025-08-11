using Application.DTOs;

namespace Application.Features.Users.Commands.Register;

public record RegisterCommand(
    string UserName,
    string Email,
    string Password,
    string FullName,
    string PhoneNumber,
    AddressDto Address,
    int? Age) : ICommand;