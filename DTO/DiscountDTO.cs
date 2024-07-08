using System.ComponentModel.DataAnnotations;

namespace ecommerce.DTO
{
  public class DiscountDTO
  {
    [Required]
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1,100, ErrorMessage = "Discount percentage has to be betweeen 1 and 100")]
    public decimal Percentage { get; set; }

    [Required]
    public DateTime DiscountStarts { get; set; }

    [Required]
    public DateTime DiscountEnds { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }

  public class CreateDiscountDTO
  {
    [Required]
    public int ProductId { get; set; }

    [Required]
    public decimal Percentage { get; set; }

    [Required]
    public DateTime DiscountStarts { get; set; }

    [Required]
    public DateTime DiscountEnds { get; set; }
  }
}