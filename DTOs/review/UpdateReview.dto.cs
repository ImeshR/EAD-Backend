using System.ComponentModel.DataAnnotations;

namespace EAD_Backend.DTOs
{
    public class UpdateReviewDto
    {
        [Required]
        public string ProductId { get; set; } = string.Empty;

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public double Rating { get; set; } // Rating given by the customer

        [Required]
        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
        public string Comment { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
