using System.ComponentModel.DataAnnotations;

namespace ecommerce.DTO
{
  public class DiscountDTO
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public DateTime DiscountStarts { get; set; }
    public DateTime DiscountEnds { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public decimal Percentage { get; set; }
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