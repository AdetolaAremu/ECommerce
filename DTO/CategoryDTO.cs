using System.ComponentModel.DataAnnotations;
using ecommerce.Models;

namespace ecommerce.DTO
{
  public class CategoryDTO
  {
    [Required]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }
  }

  public class CreateCategoryDTO
  {
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    
    [Required]
    public bool Visible { get; set; }
  }
}