using ecommerce.Helpers;
using ecommerce.Models;
using ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
  [Authorize]
  [ApiController]
  [Route("/api/orders")]
  public class OrderController : ControllerBase
  {
    private IOrderRespository _orderRespository;
    private ResponseHelper _responseHelper;
    private IShoppingCartRepository _shoppingCartRepository;
    private IUserRepository _userRepository;
    private IDiscountRepository _discountRepository;
    private ICouponRepository _couponRepository;
    private readonly ILogger<OrderController> _logger;
    private ICouponUsageRepository _couponUsageRepository;

    public OrderController(IOrderRespository orderRespository, ResponseHelper responseHelper, IShoppingCartRepository shoppingCartRepository,
      IUserRepository userRepository, IDiscountRepository discountRepository, ICouponRepository couponRepository, ILogger<OrderController> logger,
      ICouponUsageRepository couponUsageRepository
    )
    {
      _orderRespository = orderRespository;
      _responseHelper = responseHelper;
      _shoppingCartRepository = shoppingCartRepository;
      _userRepository = userRepository;
      _discountRepository = discountRepository;
      _couponRepository = couponRepository;
      _logger = logger;
      _couponUsageRepository = couponUsageRepository;
    }

    [HttpPost("checkout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CheckOut([FromQuery] string? couponCode) 
    {
      var user = _userRepository.GetLoggedInUser();

      var cartItems = _shoppingCartRepository.GetLoggedInUserCart(user.Id);

      if (cartItems == null) return _responseHelper.ErrorResponseHelper<string>("Cart item is empty");

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState);

      var cartLists = new List<CartItem>();

      foreach (var item in cartItems)
      {
        // cart items
        var checkDiscount = _discountRepository.GetProductDiscount(item.ProductId);

        if (checkDiscount != null)
        {
          item.Price = item.Price * (1 - checkDiscount.Percentage / 100);
        }

        cartLists.Add(item);
      }

      Coupon coupon= null;

      if (!String.IsNullOrEmpty(couponCode)) {
        coupon = _couponRepository.CheckCouponCode(couponCode);
      }

      var checkoutItems = _orderRespository.CheckOut(user.Id, cartLists, coupon);

      if (!checkoutItems) return _responseHelper.ErrorResponseHelper<string>("Unable to checkout due to some issues", null, 500);

      _shoppingCartRepository.ClearShoppingCart(user.Id);
      
      return _responseHelper.SuccessResponseHelper("Item added to cart successfully", cartLists, 201);
    }

    [HttpGet("logged-in-user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetLoggedInUserOrders([FromQuery] int pageNumber = 1, int pageSize = 15)
    {
      var user = _userRepository.GetLoggedInUser();

      var orders = _orderRespository.GetLoggedInUserOrders(user.Id, pageNumber, pageSize);

      return _responseHelper.SuccessResponseHelper("Orders retrieved successfully", orders);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetAllOrders([FromQuery] int pageNumber = 1, int pageSize = 15)
    {
      if (!_userRepository.CheckIfUserIsAnAdmin()) return _responseHelper.ErrorResponseHelper<string>("You are not authorized", null, 401);

      var orders = _orderRespository.GetAllOrders(pageNumber, pageSize);

      return _responseHelper.SuccessResponseHelper("Orders retrieved successfully", orders);
    }

    [HttpGet("{orderId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetOneOrder(int orderId)
    {
      if (!_orderRespository.CheckIfOrderExists(orderId)) return _responseHelper.ErrorResponseHelper<string>("Order does not exist", null, 404);

      var order = _orderRespository.GetOneOrder(orderId);

      return _responseHelper.SuccessResponseHelper("Order retrieved successfully", order);
    }
  }
}