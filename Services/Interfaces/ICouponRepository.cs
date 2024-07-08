using System.ComponentModel.DataAnnotations;

namespace ecommerce.Services.Interfaces
{
  public class ICouponRepository
  {
    [Required]
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    [StringLength(100)]
    public string Code { get; set; }

    [Required]
    public DateTime DiscountStarts { get; set; }
    public DateTime? DiscountEnds { get; set; }
  }

  public class UpdateICouponRepository
  {
    [Required]
    public int ProductId { get; set; }

    [Required]
    [StringLength(100)]
    public string Code { get; set; }

    [Required]
    public DateTime DiscountStarts { get; set; }
    public DateTime? DiscountEnds { get; set; }
  }
}