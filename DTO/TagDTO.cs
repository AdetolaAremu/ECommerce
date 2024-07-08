using System.ComponentModel.DataAnnotations;

namespace ecommerce.DTO
{
  public class TagDTO
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Visible { get; set; }
  }

  public class CreateTagDTO
  {
    [Required]
    [StringLength(150)]
    public string Name { get; set; }

    [Required]
    public bool Visible { get; set; }
  }
}