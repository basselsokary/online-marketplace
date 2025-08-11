using System.ComponentModel.DataAnnotations;

namespace Web.Models.Reviews;

public class EditReviewViewModel
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [Range(1,5, ErrorMessage = "Rating must be between 1 and 5.")]
    public int Rating { get; set; }

    [StringLength(maximumLength: 1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
    public string? Comment { get; set; }
}
