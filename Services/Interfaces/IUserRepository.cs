using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface IUserRepository
  {
    // get all users
    IEnumerable<User> GeAllUsers(int pageSize, int pageNumber);

    // check if user an admin
    bool CheckIfUserIsAnAdmin(int userId);

    // check if user exists
    bool CheckIfUserExist(int userId);

    // get logged In User
    // User GetLoggedInUser();
  }
}