using ecommerce.DTO;
using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface IShoppingCartRepository
  {
    // get logged in user cart
    ShoppingCart GetLoggedInUserCart();

    // add to cart items
    bool AddItemtoShoppingCart(CartItemDTO cartItemDTO);

    // cart item exist (product id)
    bool CheckIfItemExists(int productId);

    // remove items from shoppingCart
    bool ClearShoppingCart();

    // remove an items from cart
    bool removeItemFromShoppingCart(int productId);

    // save transaction
    bool SaveTransaction();
  }
}