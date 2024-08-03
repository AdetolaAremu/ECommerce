using ecommerce.DTO;
using ecommerce.Helpers;
using ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
  [ApiController]
  [Route("/api/discounts")]
  public class DiscountController : ControllerBase
  {
    private IDiscountRepository _discountRepository;
    private ResponseHelper _responseHelper;
    private IProductRepository _productRepository;
    private IUserRepository _userRepository;

    public DiscountController(IDiscountRepository discountRepository, ResponseHelper responseHelper, IProductRepository productRepository,
      IUserRepository userRepository
    )
    {
      _discountRepository = discountRepository;
      _responseHelper = responseHelper;
      _productRepository = productRepository;
      _userRepository = userRepository;
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetAllDiscounts([FromQuery] int pageNumber = 1, int pageSize = 20)
    {
      if (!_userRepository.CheckIfUserIsAnAdmin()) return _responseHelper.ErrorResponseHelper<string>("User is not authorized", null, 401);

      if (pageNumber <= 0) pageNumber = 1;
      if (pageSize <= 0) pageSize = 20;

      var discounts = _discountRepository.GetAllDiscounts(pageNumber, pageSize);

      return _responseHelper.SuccessResponseHelper("Discounts retrieved successfully", discounts);
    }

    [HttpGet("{productId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetProductDiscount(int productId)
    {
      if (!_productRepository.CheckIfProductExists(productId)) return _responseHelper.ErrorResponseHelper<string>("Product does not exists", null, 404);

      if (!_discountRepository.DiscountExistsForProduct(productId)) return _responseHelper.SuccessResponseHelper<string>("Discount does not exists for product", null, 400);

      var discount =  _discountRepository.GetProductDiscount(productId);

      return _responseHelper.SuccessResponseHelper("Discount retrieved successfully", discount);
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateDiscountDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult CreateDiscount([FromBody] CreateDiscountDTO createDiscountDTO)
    {
      if (!_userRepository.CheckIfUserIsAnAdmin()) return _responseHelper.ErrorResponseHelper<string>("User is not authorized", null, 401);

      if (createDiscountDTO == null) return _responseHelper.ErrorResponseHelper<string>("Request body cannot be empty");

      // if (!_discountRepository.DiscountExistsForProduct(createDiscountDTO.ProductId)) return _responseHelper.SuccessResponseHelper<string>("Discount does not exists for product", null, 400);

      var createDiscount = _discountRepository.CreateDiscount(createDiscountDTO);

      if (!createDiscount) return _responseHelper.ErrorResponseHelper<string>("Unable to create discount due to some issues", null, 500);

      return _responseHelper.SuccessResponseHelper("Discount created successfully", createDiscount, 201);
    }

    [Authorize]
    [HttpPut("{discountId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateDiscountDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult UpdateDiscount(DiscountDTO discountDTO, int discountId)
    {
      if (!_userRepository.CheckIfUserIsAnAdmin()) return _responseHelper.ErrorResponseHelper<string>("User is not authorized", null, 401);

      if (discountDTO == null) return _responseHelper.ErrorResponseHelper<string>("Request body cannot be empty");

      if (!_discountRepository.DiscountExistsForProduct(discountDTO.ProductId)) return _responseHelper.SuccessResponseHelper<string>("Discount does not exists for product", null, 400);

      var updateDiscount = _discountRepository.UpdateDiscount(discountDTO, discountId);

      if (!updateDiscount) return _responseHelper.ErrorResponseHelper<string>("Unable to update discount due to some issues", null, 500);

      return _responseHelper.SuccessResponseHelper<string>("Discount updated successfully", null);
    }

    [Authorize]
    [HttpDelete("{discountId}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult DeleteDiscount(int discountId)
    {
      if (!_userRepository.CheckIfUserIsAnAdmin()) return _responseHelper.ErrorResponseHelper<string>("User is not authorized", null, 401);

      if (!_discountRepository.DiscountExists(discountId)) return _responseHelper.ErrorResponseHelper<string>("Discount does not exists", null, 404);

      var discount = _discountRepository.GetDiscount(discountId);

      var deleteDiscount = _discountRepository.DeleteDiscount(discount);

      if (!deleteDiscount) return _responseHelper.ErrorResponseHelper<string>("Unable to delete discount due to some issues", null, 500);

      return _responseHelper.SuccessResponseHelper<string>("Discount deleted", null);
    }
  }
}