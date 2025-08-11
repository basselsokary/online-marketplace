using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Customers;

public class CustomerUserViewModel
{
    public string? UserName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public DateTime DateJoined { get; set; }

    [Required(ErrorMessage = "Full name is required")]
    [StringLength(127, ErrorMessage = $"Full name cannot exceed 127 characters")]
    public string FullName { get; set; } = null!;

    [Required]
    public string Street { get; set; } = null!;
    [Required]
    public string District { get; set; } = null!;
    [Required]
    public string City { get; set; } = null!;
    public string? ZipCode { get; set; }

    [Range(16, 120, ErrorMessage = "Age must be at least 16")]
    public int? Age { get; set; }

    [StringLength(13, MinimumLength = 13, ErrorMessage = "Phone number length must equal 13")]
    [DisplayName("Phone *")]
    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^\+201[0125]\d*$", ErrorMessage = "Phone number must be digits only and start with +201")]
    public string PhoneNumber { get; set; } = null!;
}