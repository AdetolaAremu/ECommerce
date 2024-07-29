using System.Security.Claims;
using ecommerce.DataStore;
using ecommerce.DTO;
using ecommerce.Helpers;
using ecommerce.Models;
using ecommerce.Services.Auth;
using ecommerce.Services.Interfaces;

namespace ecommerce.Services
{
  public class UserRepository : IUserRepository
  {
    private ApplicationDBContext _applicationDBContext;
    private AuthService _authService;
    private IHttpContextAccessor _httpContextAccessor;

    public UserRepository(ApplicationDBContext applicationDBContext, AuthService authService, IHttpContextAccessor httpContextAccessor)
    {
      _applicationDBContext = applicationDBContext;
      _authService = authService;
      _httpContextAccessor = httpContextAccessor;
    }

    public IEnumerable<User> GeAllUsers(int pageSize, int pageNumber)
    {
      return _applicationDBContext.Users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }

    public UserDTO GetOneUser(int userId)
    {
      var getOneUser = _applicationDBContext.Users.Where(u => u.Id == userId).First();

      return new UserDTO(){
        Id = getOneUser.Id,
        FirstName = getOneUser.FirstName,
        LastName = getOneUser.LastName,
        Email = getOneUser.Email,
        LoginStatus = getOneUser.LoginStatus,
        UserType = getOneUser.UserType,
        avatar = getOneUser.avatar,
      };
    }

    public User GetLoggedInUser()
    {
      var getUserEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      
      if (String.IsNullOrEmpty(getUserEmail)) return null;

      return _applicationDBContext.Users.Where(u => u.Email.ToLower() == getUserEmail).First();
    }

    public bool ChangeLoginStatus(int userId)
    {
      var user = _applicationDBContext.Users.Where(u => u.Id == userId).First();

      if (user.LoginStatus) {
        user.LoginStatus = false;
      }

      if (!user.LoginStatus) {
        user.LoginStatus = true;
      }

      return SaveTransaction();
    }

    public bool CheckIfUserIsAnAdmin()
    {
      var user = GetLoggedInUser();
      
      return user.UserType == 0 ? true : false;
    }

    public bool CheckIfUserExist(int userId)
    {
      return _applicationDBContext.Users.Any(u => u.Id == userId);
    }

    public bool Register(CreateUserDTO createUserDTO)
    {
      var gethashedPassword = _authService.HashString(createUserDTO.Password);

      var user = new User(){
        FirstName = createUserDTO.FirstName,
        LastName = createUserDTO.LastName,
        Email = createUserDTO.Email,
        Password = gethashedPassword,
        UserType = createUserDTO.UserType
      };

      _applicationDBContext.Add(user);

      return SaveTransaction();
    }

    public string LoginUser(UserLoginDTO userLoginDTO)
    {
      var user = _applicationDBContext.Users.Where(u => u.Email.ToLower() == userLoginDTO.Email.ToLower()).First();
      var checkHashedPassword = _authService.verifyHashedString(user.Password, userLoginDTO.Password);
      
      if (!checkHashedPassword)
      {
        return null;
      }

      return _authService.GenerateToken(user.Id, user.Email);
    }

    public bool CheckIfEmailExists(string email)
    {
      return _applicationDBContext.Users.Any(u => u.Email.ToLower() == email.ToLower());
    }

    public bool UploadAvatar(string avatar)
    {
      var user = GetLoggedInUser();

      user.avatar = avatar;

      return SaveTransaction();
    }

    public bool SaveTransaction()
    {
      var save = _applicationDBContext.SaveChanges();
      return save >= 0 ? true : false;
    }
  }
}