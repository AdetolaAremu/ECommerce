using ecommerce.DTO;
using ecommerce.Helpers;
using ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
  [Authorize]
  [ApiController]
  [Route("/api/carts")]
  public class CartContoller : ControllerBase
  {
    private IShoppingCartRepository _shoppingCartRepository;
    private ResponseHelper _responseHelper;
    private IUserRepository _userRepository;
    private IProductRepository _productRepository;

    public CartContoller(IShoppingCartRepository shoppingCartRepository, ResponseHelper responseHelper, IUserRepository userRepository,
      IProductRepository productRepository 
    )
    {
      _shoppingCartRepository = shoppingCartRepository;
      _responseHelper = responseHelper;
      _userRepository = userRepository;
      _productRepository = productRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetLoggedInUserCart()
    {
      var user = _userRepository.GetLoggedInUser();

      var userCart = _shoppingCartRepository.GetLoggedInUserCart(user.Id);

      return _responseHelper.SuccessResponseHelper("Cart retrieved successfully", userCart);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult AddProductToCart([FromBody] CartItemDTO cartItemDTO)
    {
      if (cartItemDTO == null) return _responseHelper.ErrorResponseHelper<string>("Request body cannot be empty");

      var user = _userRepository.GetLoggedInUser();

      if (_shoppingCartRepository.CheckIfProductExistsInCartItem(cartItemDTO.ProductId, user.Id)) 
        return _responseHelper.ErrorResponseHelper<string>("Product already exist in your shopping cart");

      if (!_productRepository.CheckIfProductExists(cartItemDTO.ProductId)) return _responseHelper.ErrorResponseHelper<string>("Product does not exists");

      var cart = _shoppingCartRepository.AddItemtoShoppingCart(cartItemDTO, user.Id);

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState);

      if (!cart) return _responseHelper.ErrorResponseHelper<string>("Unable to add product to cart due to some issues", null, 500);
      
      return _responseHelper.SuccessResponseHelper("Item added to cart successfully", cart, 201);
    }

    [HttpDelete("/clear-cart")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult ClearShoppingCart()
    {
      var user = _userRepository.GetLoggedInUser();

      var cart = _shoppingCartRepository.ClearShoppingCart(user.Id);

      if (!cart) return _responseHelper.ErrorResponseHelper<string>("Unable to clear cart due to some issues", null, 500);

      return _responseHelper.SuccessResponseHelper<string>("Cart cleared successfully", null);
    }

    [HttpDelete("/delete/{productId}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult RemoveItemFromShoppingCart(int productId)
    {
      var user = _userRepository.GetLoggedInUser();

      if (!_productRepository.CheckIfProductExists(productId)) return _responseHelper.ErrorResponseHelper<string>("Product does not exists");

      if (!_shoppingCartRepository.CheckIfProductExistsInCartItem(productId, user.Id)) 
        return _responseHelper.ErrorResponseHelper<string>("Product does not exist in your shopping cart");      

      var cart = _shoppingCartRepository.RemoveItemFromShoppingCart(productId, user.Id);

      if (!cart) return _responseHelper.ErrorResponseHelper<string>("Unable to delete cart item due to some issues", null, 500);

      return _responseHelper.SuccessResponseHelper<string>("Cart deleted successfully", null);
    }
  }
}