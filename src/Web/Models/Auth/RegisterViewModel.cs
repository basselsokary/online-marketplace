using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using static Domain.Constants.DomainConstants.Customer;

namespace Web.Models.Auth;

public class RegisterViewModel
{
    [Required(ErrorMessage = "User name is required")]
    [StringLength(50, ErrorMessage = "User name cannot exceed 50 characters")]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "Full name is required")]
    [StringLength(FullNameMaxLength, ErrorMessage = $"Full name cannot exceed 127 characters")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = null!;

    [StringLength(13, MinimumLength = 13, ErrorMessage = "Phone number length must equal 13")]
    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^\+201[0125]\d*$", ErrorMessage = "Phone number must be digits only and start with +201")]
    public string PhoneNumber { get; set; } = null!;

    public AddressDto Address { get; set; } = null!;

    [Range(16, 120, ErrorMessage = "Age must be at least 16")]
    public int? Age { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; } = null!;

    // [Required(ErrorMessage = "You must agree to the terms")]
    // public bool AgreeToTerms { get; set; }

    // public bool SubscribeToNewsletter { get; set; }
}