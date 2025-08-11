namespace Application.DTOs;

public record AddressDto(
    string Street,
    string District,
    string City,
    string? ZipCode
)
{
    public override string ToString()
    {
        return $"{Street}, {District}, {City}, {ZipCode}";
    }
}
