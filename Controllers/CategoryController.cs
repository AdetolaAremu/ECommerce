using ecommerce.DTO;
using ecommerce.Helpers;
using ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
  [Authorize]
  [ApiController]
  [Route("/api/categories")]
  public class CategoryController : ControllerBase
  {
    private ICategoryRepository _categoryRepository;
    private ResponseHelper _responseHelper;

    public CategoryController(ICategoryRepository categoryRepository, ResponseHelper responseHelper)
    {
      _categoryRepository = categoryRepository;
      _responseHelper = responseHelper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetAllCategories([FromQuery] string? searchTerm)
    {
      var categories = _categoryRepository.GetAllCategories(searchTerm);

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState);

      return _responseHelper.SuccessResponseHelper("Categories retrived successfully", categories);
    }

    [HttpGet("/{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetOneCategory(int categoryId)
    {
      if (!_categoryRepository.CategoryExists(categoryId)) return _responseHelper.ErrorResponseHelper<string>("Category does not exist", null, 404);

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState);

      var getCategory = _categoryRepository.GetOneCategory(categoryId);

      return _responseHelper.SuccessResponseHelper("Category retrieved successfully", getCategory);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateCategoryDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateCategory([FromBody] CreateCategoryDTO createCategoryDTO)
    {
      if (createCategoryDTO == null) return _responseHelper.ErrorResponseHelper<string>("Request body cannot be empty");

      if (_categoryRepository.CheckIfCategoryNameExists(createCategoryDTO.Name))
        return _responseHelper.ErrorResponseHelper<string>($"Category name: {createCategoryDTO.Name} exists");

      var createCategory = _categoryRepository.CreateCategory(createCategoryDTO);

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState);

      if (!createCategory) return _responseHelper.ErrorResponseHelper<string>("Unable to create category due to some issues", null, 500);

      return _responseHelper.SuccessResponseHelper("Category created successfully", createCategoryDTO, 201);
    }

    [HttpPut("/{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateCategoryDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateCategory(int categoryId, CreateCategoryDTO categoryDTO)
    {
      if (categoryDTO == null) return _responseHelper.ErrorResponseHelper<string>("Request body cannot be empty");

      if (_categoryRepository.CheckIfUpdateCategoryNameExists(categoryId, categoryDTO.Name))
        return _responseHelper.ErrorResponseHelper<string>($"Category name: {categoryDTO.Name} exists with an another record");

      var updateCategory = _categoryRepository.UpdateCategory(categoryId, categoryDTO);

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState);

      if (!updateCategory) return _responseHelper.ErrorResponseHelper<string>("Unable to create category due to some issues", null, 500);

      return _responseHelper.SuccessResponseHelper<string>("Category updated successfully", null);
    }

    [HttpDelete("/{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteCategory(int categoryId)
    {
      if (!_categoryRepository.CategoryExists(categoryId)) return _responseHelper.ErrorResponseHelper<string>("Category does not exist", null, 404);

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState);

      var category = _categoryRepository.GetOneCategory(categoryId);

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState);

      var deleteCategory = _categoryRepository.DeleteCategory(category);

      if (!deleteCategory) return _responseHelper.ErrorResponseHelper<string>("Unable to create category due to some issues", null, 500);

      return _responseHelper.SuccessResponseHelper<string>("Category deleted successfully", null);
    }
  }
}