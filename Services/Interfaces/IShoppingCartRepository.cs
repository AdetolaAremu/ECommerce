using ecommerce.DTO;
using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface IShoppingCartRepository
  {
    // get logged in user cart
    ShoppingCart GetLoggedInUserCart(int userId);

    // add to cart items
    bool AddItemtoShoppingCart(CreateCartItemDTO cartItemDTO, int userId);

    // cart item exist (product id)
    bool CheckIfProductExistsInCartItem(int productId, int userId);

    // remove items from shoppingCart
    bool ClearShoppingCart(int userId);

    // remove an items from cart
    bool RemoveItemFromShoppingCart(int productId, int userId);

    // save transaction
    bool SaveTransaction();
  }
}