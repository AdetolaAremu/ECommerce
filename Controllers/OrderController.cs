using ecommerce.Helpers;
using ecommerce.Models;
using ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
  public class OrderController : ControllerBase
  {
    private IOrderRespository _orderRespository;
    private ResponseHelper _responseHelper;
    private IShoppingCartRepository _shoppingCartRepository;
    private IUserRepository _userRepository;
    private IDiscountRepository _discountRepository;

    public OrderController(IOrderRespository orderRespository, ResponseHelper responseHelper, IShoppingCartRepository shoppingCartRepository,
      IUserRepository userRepository, IDiscountRepository discountRepository
    )
    {
      _orderRespository = orderRespository;
      _responseHelper = responseHelper;
      _shoppingCartRepository = shoppingCartRepository;
      _userRepository = userRepository;
      _discountRepository = discountRepository;
    }

    public IActionResult CheckOut() 
    {
      // check if user has cart items at all
      var user = _userRepository.GetLoggedInUser();

      var cartItems = _shoppingCartRepository.GetLoggedInUserCart(user.Id);

      if (cartItems == null) return _responseHelper.ErrorResponseHelper<string>("Cart item is empty");

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState);

      var cartLists = new List<CartItem>();

      foreach (var item in cartItems.CartItems)
      {
        var checkDiscount = _discountRepository.GetProductDiscount(item.ProductId);

        if (checkDiscount != null) 
        {
          item.Price = item.Price * (checkDiscount.Percentage / 100);
        }

        cartLists.Add(item);
      }

      var checkoutItems = _orderRespository.CheckOut(user.Id, cartLists);

      if (!checkoutItems) return _responseHelper.ErrorResponseHelper<string>("Unable to checkout due to some issues", null, 500);
      
      return _responseHelper.SuccessResponseHelper("Item added to cart successfully", cartLists, 201);
    }
  }
}