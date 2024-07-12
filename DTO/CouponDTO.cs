using System.ComponentModel.DataAnnotations;

namespace ecommerce.DTO
{
  public class CouponDTO
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

  public class CreateCouponDTO
  {
    [Required]
    public int ProductId { get; set; }

    [StringLength(150)]
    public string Code { get; set; }
    
    [Required]
    public DateTime DiscountStarts { get; set; }
    public DateTime? DiscountEnds { get; set; }
  }
}