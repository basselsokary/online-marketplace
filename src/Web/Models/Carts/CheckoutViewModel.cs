using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using Domain.Enums;

namespace Web.Models.Carts;

public class CheckoutViewModel
{
    public AddressDto Address { get; set; } = null!;
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentDetailsViewModel? PaymentDetails { get; set; }
    public string? OrderNotes { get; set; } = string.Empty;
    public bool SaveAddress { get; set; }

    public List<CartItemViewModel> CartItems { get; set; } = [];
    public decimal Total { get; set; }
}
