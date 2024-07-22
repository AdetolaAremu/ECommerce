using ecommerce.DataStore;
using ecommerce.DTO;
using ecommerce.Models;
using ecommerce.Services.Interfaces;

namespace ecommerce.Services
{
  public class CategoryRepository : ICategoryRepository
  {
    private ApplicationDBContext _applicationDBContext;

    public CategoryRepository(ApplicationDBContext applicationDBContext)
    {
      _applicationDBContext = applicationDBContext;
    }

    public IEnumerable<Category> GetAllCategories(string? searchTerm=null)
    {
      var query = _applicationDBContext.Categories.AsQueryable();
      
      if (!string.IsNullOrEmpty(searchTerm))
      {
        query = query.Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()));
      }

      return query.ToList();
    }

    public Category GetOneCategory(int categoryId)
    {
      return _applicationDBContext.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
    }

    public bool CategoryExists(int categoryId)
    {
      return _applicationDBContext.Categories.Any(c => c.Id == categoryId);
    }

    public bool CheckIfCategoryNameExists(string name)
    {
      return _applicationDBContext.Categories.Any(c => c.Name.ToLower() == name.ToLower());
    }

    public bool CheckIfUpdateCategoryNameExists(int categoryId, string name)
    {
      return _applicationDBContext.Categories.Any(c => c.Name.ToLower() == name.ToLower() && c.Id != categoryId); 
    }

    public bool CreateCategory(CreateCategoryDTO createCategoryDTO)
    {
      var category = new Category(){
        Name = createCategoryDTO.Name
      };

      _applicationDBContext.Add(category);

      return SaveTransaction();
    }

    public bool UpdateCategory(int categoryId, CategoryDTO categoryDTO)
    {
      var getCategory = _applicationDBContext.Categories.Where(c => c.Id == categoryId).First();

      getCategory.Name = categoryDTO.Name;

      return SaveTransaction();
    }

    public bool DeleteCategory(Category category)
    {
      _applicationDBContext.Remove(category);

      return SaveTransaction();
    }

    public bool SaveTransaction()
    {
      var saveQuery = _applicationDBContext.SaveChanges();
      return saveQuery >= 0 ? true : false; 
    }
  }
}