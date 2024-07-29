using ecommerce.DTO;
using ecommerce.Helpers;
using ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
  [ApiController]
  [Route("/api/coupons")]
  public class CouponController : ControllerBase
  {
    private ICouponRepository _couponRepository;
    private IProductRepository _productRepository;
    private ResponseHelper _responseHelper;
    private IUserRepository _userRepository;

    public CouponController(ICouponRepository couponRepository, IProductRepository productRepository, ResponseHelper responseHelper, IUserRepository userRepository)
    {
      _couponRepository = couponRepository;
      _productRepository = productRepository;
      _responseHelper = responseHelper;
      _userRepository = userRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetAllCoupons([FromQuery] int pageNumber = 1, int pageSize = 10)
    {
      if (!_userRepository.CheckIfUserIsAnAdmin()) return _responseHelper.ErrorResponseHelper<string>("User is not an admin", null, 401);

      if (pageNumber <= 0) pageNumber = 1;
      if (pageSize <= 0) pageSize = 1;

      var coupons = _couponRepository.GetAllCoupons(pageSize, pageNumber);

      return _responseHelper.SuccessResponseHelper("Coupons retrieved successfully", coupons);
    }

    [HttpGet("{couponId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetOneCoupon(int couponId)
    {
      if (!_couponRepository.CouponExists(couponId)) return _responseHelper.ErrorResponseHelper<string>("Coupon does not exist", null, 404);

      var coupon = _couponRepository.GetOneCoupon(couponId);

      return _responseHelper.SuccessResponseHelper("Coupon retrieved successfully", coupon);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult CreateProductCoupon([FromBody] CreateCouponDTO createCouponDTO)
    {
      if (!_userRepository.CheckIfUserIsAnAdmin()) return _responseHelper.ErrorResponseHelper<string>("User is not an admin", null, 401);

      if (createCouponDTO == null) return _responseHelper.ErrorResponseHelper<string>("Request body is empty");    

      var createCoupon = _couponRepository.CreateCoupon(createCouponDTO);

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState);

      if (!createCoupon) return _responseHelper.ErrorResponseHelper<string>("Unable to create coupon due to some issues", null, 500);

      return _responseHelper.SuccessResponseHelper("Coupon created successfully", createCouponDTO, 201);
    }

    [HttpPut("{couponId}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateProductCoupon(int couponId, CouponDTO couponDTO)
    {
      if (!_userRepository.CheckIfUserIsAnAdmin()) return _responseHelper.ErrorResponseHelper<string>("User is not an admin", null, 401);

      if (couponDTO == null) return _responseHelper.ErrorResponseHelper<string>("Request body is empty");    

      if (!_couponRepository.CouponExists(couponId)) return _responseHelper.ErrorResponseHelper<string>("Coupon does not exist", null, 404);

      var couponUpdate = _couponRepository.UpdateCoupon(couponId, couponDTO);

      if (!couponUpdate) return _responseHelper.ErrorResponseHelper<string>("Unable to create coupon due to some issues", null, 500);

      return _responseHelper.SuccessResponseHelper<string>("Coupon updated successfully", null);
    }

    public IActionResult GetCouponsForAProduct(int productId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
      if (!_userRepository.CheckIfUserIsAnAdmin()) return _responseHelper.ErrorResponseHelper<string>("User is not an admin", null, 401);

      if (!_productRepository.CheckIfProductExists(productId)) return _responseHelper.ErrorResponseHelper<string>("Product does not exist", null, 404);
      
      if (pageNumber <= 0) pageNumber = 1;
      if (pageSize <= 0) pageSize = 1;

      var coupons = _couponRepository.GetAllCouponsPerProduct(productId, pageSize, pageNumber);

      return _responseHelper.SuccessResponseHelper("Coupons retrieved successfully", coupons);
    }

    public IActionResult DeleteProduct(int couponId)
    {
      if (!_couponRepository.CouponExists(couponId)) return _responseHelper.ErrorResponseHelper<string>("Coupon does not exist", null, 404);

      var coupon = _couponRepository.GetOneCoupon(couponId);

      var deleteCoupon = _couponRepository.DeleteCoupon(coupon);

      if (!deleteCoupon) return _responseHelper.ErrorResponseHelper<string>("Unable to delete coupon due to some issues", null, 500);

      return _responseHelper.SuccessResponseHelper<string>("Coupon deleted successfully", null);
    }
  }
}