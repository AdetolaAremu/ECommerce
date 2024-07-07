using System.ComponentModel.DataAnnotations;

namespace ecommerce.DTO
{
  public class GetCouponUsageDTO
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int UserId { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }

  public class CreateCouponUsageDTO
  {
    [Required]
    public int ProductId { get; set; }

    [Required]
    public int UserId { get; set; }
  }
}