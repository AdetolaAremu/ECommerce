using System.ComponentModel.DataAnnotations;

namespace ecommerce.DTO
{
  public class GetCouponDTO
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Code { get; set; }
    
    public DateTime DiscountStarts { get; set; }
    public DateTime? DiscountEnds { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
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