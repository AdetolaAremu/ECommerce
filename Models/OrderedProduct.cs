using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce.Models
{
  public class OrderedProduct
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Title {get; set;}

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; }

    public int productId { get; set; }
  }
}