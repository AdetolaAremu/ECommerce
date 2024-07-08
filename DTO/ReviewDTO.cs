using System.ComponentModel.DataAnnotations;

namespace ecommerce.DTO
{
  public class ReviewDTO
  {
    public int Id { get; set; }
    public decimal Rating { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
  }

  public class CreateReviewDTO
  {
    [Required]
    [Range(1, 5, ErrorMessage = "Rating has to be between 1 and 5")]
    public decimal Rating { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int ProductId { get; set; }
  }
}