using ecommerce.DTO;
using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface ICategoryRepository
  {
    // get all categories. Search param
    IEnumerable<Category> GetAllCategories(string searchTerm);

    // get Once Category
    Category GetOneCategory(int categoryId);

    // check if category exists
    bool CategoryExists(int categoryId);

    // Create category
    bool CreateCategory(CreateCategoryDTO createCategoryDTO);

    // update category
    bool UpdateCategory(int categoryId, CategoryDTO categoryDTO);

    // delete category
    bool DeleteCategory(Category category);

    // save category
    bool SaveTransaction();
  }
}