using ecommerce.DTO;
using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface IUserRepository
  {
    // get all users
    IEnumerable<User> GeAllUsers(int pageSize, int pageNumber);

    // check if user an admin
    bool CheckIfUserIsAnAdmin(int userId);

    // register a new user
    bool Register(CreateUserDTO createUserDTO);

    // login
    string LoginUser(UserLoginDTO userLoginDTO);

    // check if email exist
    bool CheckIfEmailExists(string email);

    // check if user exists
    bool CheckIfUserExist(int userId);

    // save transaction
    bool SaveTransaction();

    // get logged In User
    // User GetLoggedInUser();
  }
}