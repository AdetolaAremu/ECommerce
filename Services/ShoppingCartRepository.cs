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

    public IEnumerable<CartItem> GetLoggedInUserCart(int userId)
    {
      return _applicationDBContext.ShoppingCarts.Where(sc => sc.UserId == userId).SelectMany(sc => sc.CartItems).ToList();
    }

    public bool CreateShoppingCart(CreateCartItemDTO cartItemDTO, int userId)
    {
      var shoppingCart = new ShoppingCart(){
        UserId = userId
      };

      _applicationDBContext.Add(shoppingCart);

      SaveTransaction();

      var product = _applicationDBContext.Products.Where(p => p.Id == cartItemDTO.ProductId).First();

      var cartItem = new CartItem(){
        ProductId = cartItemDTO.ProductId,
        Price = product.Price,
        Quantity = cartItemDTO.Quantity,
        Title = product.Title,
        ShoppingCartId = shoppingCart.Id
      };

      _applicationDBContext.Add(cartItem);

      return SaveTransaction();
    }

    public bool AddItemtoShoppingCart(CreateCartItemDTO cartItemDTO, int userId)
    {
      var checkCart = _applicationDBContext.ShoppingCarts.Where(sc => sc.UserId == userId).FirstOrDefault();

      if(checkCart == null)
      {
        return CreateShoppingCart(cartItemDTO, userId); 
      }

      var product = _applicationDBContext.Products.Where(p => p.Id == cartItemDTO.ProductId).First();

      var userCartItem = new CartItem(){
        ProductId = cartItemDTO.ProductId,
        Price = product.Price,
        Quantity = cartItemDTO.Quantity,
        Title = product.Title,
        ShoppingCartId = checkCart.Id
      };

      _applicationDBContext.Add(userCartItem);

      return SaveTransaction();
    }

    public bool CheckIfProductExistsInCartItem(int productId, int userId)
    {
      return _applicationDBContext.CartItems.Any(sc => sc.ShoppingCart.UserId == userId && sc.ProductId == productId);
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