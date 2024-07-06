using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ecommerce.Enums;

namespace ecommerce.Models
{
  public class Order
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public OrderStatusEnum Status { get; set; } = OrderStatusEnum.pending;

    public decimal Amount { get; set; } // total amount for all products
    public decimal DeliveryFees { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public ICollection<Product> Products { get; set; }
  }
}