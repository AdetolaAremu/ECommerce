using System.ComponentModel.DataAnnotations;
using ecommerce.Models;

namespace ecommerce.DTO
{
  public class GetCategoryDTO
  {
    public int Id { get; set; }
    public int Name { get; set; }
    public bool Visible { get; set; }
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