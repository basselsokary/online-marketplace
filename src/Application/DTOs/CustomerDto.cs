namespace Application.DTOs;

public record CustomerDto(
    string Id,
    string UserName,
    string FullName,
    string Email,
    int? Age,
    AddressDto Address,
    string PhoneNumber,
    DateTime DateJoined
);
