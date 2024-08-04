using ecommerce.DTO;
using ecommerce.Helpers;
using ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
  [ApiController]
  [Authorize]
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
    public IActionResult CreateCoupon([FromBody] CreateCouponDTO createCouponDTO)
    {
      if (!_userRepository.CheckIfUserIsAnAdmin()) return _responseHelper.ErrorResponseHelper<string>("User is not an admin", null, 401);

      if (createCouponDTO == null) return _responseHelper.ErrorResponseHelper<string>("Request body is empty");    

      var createCoupon = _couponRepository.CreateCoupon(createCouponDTO);

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState, 500);

      if (!createCoupon) return _responseHelper.ErrorResponseHelper<string>("Unable to create coupon due to some issues", null, 500);

      return _responseHelper.SuccessResponseHelper("Coupon created successfully", createCouponDTO, 201);
    }

    [HttpPut("{couponId}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateCoupon(int couponId, CouponDTO couponDTO)
    {
      if (!_userRepository.CheckIfUserIsAnAdmin()) return _responseHelper.ErrorResponseHelper<string>("User is not an admin", null, 401);

      if (couponDTO == null) return _responseHelper.ErrorResponseHelper<string>("Request body is empty");    

      if (!_couponRepository.CouponExists(couponId)) return _responseHelper.ErrorResponseHelper<string>("Coupon does not exist", null, 404);

      var couponUpdate = _couponRepository.UpdateCoupon(couponId, couponDTO);

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState, 500);

      if (!couponUpdate) return _responseHelper.ErrorResponseHelper<string>("Unable to create coupon due to some issues", null, 500);

      return _responseHelper.SuccessResponseHelper<string>("Coupon updated successfully", null);
    }

    [HttpDelete("{couponId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteCoupon(int couponId)
    {
      if (!_userRepository.CheckIfUserIsAnAdmin()) return _responseHelper.ErrorResponseHelper<string>("User is not an admin", null, 401);

      if (!_couponRepository.CouponExists(couponId)) return _responseHelper.ErrorResponseHelper<string>("Coupon does not exist", null, 404);

      var coupon = _couponRepository.GetOneCoupon(couponId);

      var deleteCoupon = _couponRepository.DeleteCoupon(coupon);

      if (!deleteCoupon) return _responseHelper.ErrorResponseHelper<string>("Unable to delete coupon due to some issues", null, 500);

      return _responseHelper.SuccessResponseHelper<string>("Coupon deleted successfully", null);
    }

    // we will need this when we are about to checkout too
    [HttpGet("/validity")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CheckCouponValidity([FromQuery] int productId, string code)
    {
      // check if product exists
      if (!_productRepository.CheckIfProductExists(productId)) return _responseHelper.ErrorResponseHelper<string>("Product does not exist", null, 404);

      // check if any coupon matches the incoming code
      var checkCoupon = _couponRepository.CheckCouponCode(code);

      if (checkCoupon == null) return _responseHelper.ErrorResponseHelper<string>("Coupon code is not correct or does not exist");

      // check expiry
      if (_couponRepository.CheckCouponExpiry(checkCoupon)) return _responseHelper.ErrorResponseHelper<string>("Coupon has expired");

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState, 500);

      return _responseHelper.SuccessResponseHelper<string>("Coupon is valid", null);
    }
  }
}