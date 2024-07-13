using ecommerce.DataStore;
using ecommerce.DTO;
using ecommerce.Models;
using ecommerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Services
{
  public class ShoppingCartRepository : IShoppingCartRepository
  {
    private ApplicationDBContext _applicationDBContext;

    public ShoppingCartRepository(ApplicationDBContext applicationDBContext)
    {
      _applicationDBContext = applicationDBContext;
    }

    public ShoppingCart GetLoggedInUserCart(int userId)
    {
      return _applicationDBContext.ShoppingCarts.Where(sc => sc.UserId == userId).Include(ci => ci.CartItems).FirstOrDefault();
    }

    public bool AddItemtoShoppingCart(CartItemDTO cartItemDTO, int userId)
    {
      var checkCart = CheckIfProductExistsInCartItem(cartItemDTO.ProductId, userId);

      ShoppingCart shoppingCart = null;

      if(!checkCart)
      {
        shoppingCart = new ShoppingCart(){
          UserId = userId
        };

        _applicationDBContext.Add(shoppingCart);

        SaveTransaction();
      }

      var cartItem = new CartItem(){
        ProductId = cartItemDTO.ProductId,
        ShoppingCartId = shoppingCart.Id
      };

      _applicationDBContext.Add(cartItem);

      return SaveTransaction();
    }

    public bool CheckIfProductExistsInCartItem(int productId, int userId)
    {
      return _applicationDBContext.ShoppingCarts.Any(sc => sc.UserId == userId);
    }

    public bool ClearShoppingCart(int userId)
    {
      var shoppingCart = GetLoggedInUserCart(userId);

      _applicationDBContext.Remove(shoppingCart);

      return SaveTransaction();
    }

    public bool RemoveItemFromShoppingCart(int productId, int userId)
    {
      var cartItem = _applicationDBContext.CartItems.Where(ci => ci.ProductId == productId && ci.ShoppingCart.UserId == userId).First();

      _applicationDBContext.Remove(cartItem);

      return SaveTransaction();
    }

    public bool SaveTransaction()
    {
      var shoppingCart = _applicationDBContext.SaveChanges();

      return shoppingCart >= 0 ? true : false;
    }
  }
}