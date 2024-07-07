using System.ComponentModel.DataAnnotations;
using ecommerce.Enums;

namespace ecommerce.DTO
{
  public class OrderDTO
  {
    public int Id { get; set; }
    public OrderStatusEnum Status { get; set; }
    public decimal Amount { get; set; }
    public decimal DeliveryFees { get; set; }
    public int UserId { get; set; }
  }

  public class CreateOrderDTO
  {
    [Required]
    public OrderStatusEnum Status { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public decimal DeliveryFees { get; set; }

    [Required]
    public int UserId { get; set; }
  }
}