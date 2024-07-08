using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface IOrderRespository
  {
    // Get All Orders
    IEnumerable<Order> GetAllOrders();

    // Get All Orders by logged in user
    IEnumerable<Order> GetLoggedInUserOrders(int orderId);

    // Get one order, alongside orderedItems
    Order GetOneOrder(int orderId);
  }
}