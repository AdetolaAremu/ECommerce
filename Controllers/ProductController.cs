using ecommerce.DTO;
using ecommerce.Helpers;
using ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
  [ApiController]
  [Route("/api/products")]
  public class ProductController : ControllerBase
  {
    private IProductRepository _productRepository;
    private ICategoryRepository _categoryRepository;
    private ResponseHelper _responseHelper;
    private IUserRepository _userRepository;
    private Helper _helper;

    public ProductController(IProductRepository productRepository, ResponseHelper responseHelper, ICategoryRepository categoryRepository, 
      IUserRepository userRepository, Helper helper
    )
    {
      _productRepository = productRepository;
      _responseHelper = responseHelper;
      _categoryRepository = categoryRepository;
      _userRepository = userRepository;
      _helper = helper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetAllProducts([FromQuery] string? searchTerm, string? sortBy, int pageNumber = 1, int itemSize = 20)
    {
      var products = _productRepository.GetAllProducts(searchTerm, sortBy, pageNumber, itemSize);

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState);

      return _responseHelper.SuccessResponseHelper("Products retrieved successfully", products);
    }

    [HttpGet("/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetOneProduct(int productId)
    {
      if (!_productRepository.CheckIfProductExists(productId))
        return _responseHelper.ErrorResponseHelper<string>("Product does not exist", null, 404);

      var product = _productRepository.GetOneProduct(productId);

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState);
      
      return _responseHelper.SuccessResponseHelper("Product retrieved successfully", product);
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct([FromForm]CreateProductDTO createProductDTO, IFormFile coverImage, IFormFile[] images)
    {
      if (createProductDTO == null) return _responseHelper.ErrorResponseHelper<string>("Request body can not be empty");

      if (coverImage == null || !images.Any()) return _responseHelper.ErrorResponseHelper<string>("Both cover image and product images are required");

      var coverImageUpload = _helper.SingleImageUpload(coverImage, "uploads/product-cover-image");

      var bulkImages = await _helper.BulkImageUpload(images, "uploads/product-images");

      var product = _productRepository.CreateProduct(createProductDTO, coverImageUpload, bulkImages.ToList());

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState);

      if (!product) return _responseHelper.ErrorResponseHelper<string>("Unable to create product due to some issues", null, 500);

      return _responseHelper.SuccessResponseHelper("Product created successfully", product, 201);
    }

    [Authorize]
    [HttpPut("/{productId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProduct(int productId, [FromForm] CreateProductDTO productDTO, IFormFile? coverImage, IFormFile?[] images)
    {
      if (productDTO == null) return _responseHelper.ErrorResponseHelper<string>("Request body can not be empty");

      if (!_productRepository.CheckIfProductExists(productId))
        return _responseHelper.ErrorResponseHelper<string>("Product does not exist", null, 404);

      string coverImageUpload = null;
      if (coverImage != null) {
        coverImageUpload = _helper.SingleImageUpload(coverImage, "uploads/product-cover-image");
      }

      List<string> bulkImageUpload = new List<string>();
      if (images.Any())
      {
        var nonNullableImages = images.Where(img => img != null).Cast<IFormFile>().ToArray();
        var bulkImageUploadArray = await _helper.BulkImageUpload(nonNullableImages, "uploads/product-images");
        bulkImageUpload = bulkImageUploadArray.ToList();
      }

      var updateProduct = _productRepository.UpdateProduct(productDTO, productId, coverImageUpload, bulkImageUpload);

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState);

      if (!updateProduct) return _responseHelper.ErrorResponseHelper<string>("Unable to update product due to some issues", null, 500);

      return _responseHelper.SuccessResponseHelper<string>("Product updated successfully", null);
    }

    [HttpGet("/product-category/{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetProductsByCategory(int categoryId, int pageNumber, int pageSize)
    {
      if (!_categoryRepository.CategoryExists(categoryId))
        return _responseHelper.ErrorResponseHelper<string>("Category does not exists", null, 404);
      
      var products = _productRepository.GetProductsByCategory(categoryId, pageNumber, pageSize);

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState);

      return _responseHelper.SuccessResponseHelper("Product retrieved successfully", products);
    }

    [Authorize]
    [HttpDelete("{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteProduct(int productId)
    {
      if (!_userRepository.CheckIfUserIsAnAdmin()) return _responseHelper.ErrorResponseHelper<string>("User is not an admin", null, 401);

      if (!_productRepository.CheckIfProductExists(productId))
        return _responseHelper.ErrorResponseHelper<string>("Product does not exist", null, 404);

      var product = _productRepository.GetOneProduct(productId);

      var deleteProduct = _productRepository.DeleteProduct(product);

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState);

      if (!deleteProduct) return _responseHelper.ErrorResponseHelper<string>("Unable to delete product due to some issues", null, 500);

      return _responseHelper.SuccessResponseHelper<string>("Product deleted successfully", null);
    }
  }
}