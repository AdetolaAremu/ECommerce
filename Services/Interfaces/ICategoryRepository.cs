using ecommerce.DTO;
using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface ICategoryRepository
  {
    // get all categories. Search param
    IEnumerable<Category> GetAllCategories(string? searchTerm);

    // get Once Category
    Category GetOneCategory(int categoryId);

    // check if category exists
    bool CategoryExists(int categoryId);

    // bool check if category name exists
    bool CheckIfCategoryNameExists(string name);

    // check if update category name exists
    bool CheckIfUpdateCategoryNameExists(int categoryId, string name);

    // Create category
    bool CreateCategory(CreateCategoryDTO createCategoryDTO);

    // update category
    bool UpdateCategory(int categoryId, CreateCategoryDTO createCategoryDTO);

    // delete category
    bool DeleteCategory(Category category);

    // save category
    bool SaveTransaction();
  }
}