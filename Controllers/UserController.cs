using ecommerce.DTO;
using ecommerce.Helpers;
using ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
  [Authorize]
  [ApiController]
  [Route("/api/users")]
  public class UserController : ControllerBase
  {
    private IUserRepository _userRepository;
    private ResponseHelper _responseHelper;
    private ILogger<UserController> _logger;

    public UserController(IUserRepository userRepository, ResponseHelper responseHelper, ILogger<UserController> logger)
    {
      _userRepository = userRepository;
      _responseHelper = responseHelper;
      _logger = logger;
    }

    // get Logged in user
    [HttpGet("/logged-in-user")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetLoggedInUser()
    {
      var getUser = _userRepository.GetLoggedInUser();

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper<string>($"Bad request {ModelState}");

      if (getUser == null) return _responseHelper.ErrorResponseHelper<string>("Unauthorized", null, 401);

      return _responseHelper.SuccessResponseHelper("User retrieved successfully", getUser);
    }

    // get all users
    [HttpGet("/all")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAllUsers([FromQuery] int pageSize = 20, int pageNumber = 1)
    {
      if (!_userRepository.CheckIfUserIsAnAdmin()) return _responseHelper.ErrorResponseHelper<string>("User is not an admin", null, 401);

      if (pageSize <= 0 || pageNumber <= 0) return _responseHelper.ErrorResponseHelper<string>("Query params is not correct");

      var allUsers = _userRepository.GeAllUsers(pageSize, pageNumber);

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper<string>($"Bad request {ModelState}");

      return _responseHelper.SuccessResponseHelper("Users retrieved successfully", allUsers);
    }

    // get one user
    [HttpGet("/user")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetOneUser([FromQuery] int userId)
    {
      if (!_userRepository.CheckIfUserIsAnAdmin()) return _responseHelper.ErrorResponseHelper<string>("User is not an admin", null, 401);

      if (!_userRepository.CheckIfUserExist(userId)) return _responseHelper.ErrorResponseHelper<string>("User does not exist", null, 404);

      var user = _userRepository.GetOneUser(userId);

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper<string>($"Bad request {ModelState}");

      return _responseHelper.SuccessResponseHelper("User retrieved successfully", user);
    }

    // upload image
    [HttpPut("/avatar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UploadImage(IFormFile avatar)
    {
      string avatarPath = null;
      if (avatar != null && avatar.Length > 0)
      {
        var filePath = Path.Combine("uploads", avatar.FileName);
        
        using (var stream = new FileStream(filePath, FileMode.Create)) avatar.CopyToAsync(stream);
        
        avatarPath = filePath;
      }

      var saveAvatarPath = _userRepository.UploadAvatar(avatarPath);

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper<string>($"Bad request {ModelState}");

      if (!saveAvatarPath) return _responseHelper.ErrorResponseHelper<string>("image not saved", null, 400);
          
      return _responseHelper.SuccessResponseHelper<string>("User avatar updated successfully", null);
    }

    [HttpPut("/toggle-status/{userId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult ToggleLoginStatus(int userId)
    {
      if (!_userRepository.CheckIfUserIsAnAdmin()) return _responseHelper.ErrorResponseHelper<string>("User is not an admin", null, 401);

      if (!_userRepository.CheckIfUserExist(userId)) return _responseHelper.ErrorResponseHelper<string>("User does not exit", null, 404);

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper<string>($"Bad request {ModelState}");

      var toggleStatus = _userRepository.ChangeLoginStatus(userId);

      if (!toggleStatus) return _responseHelper.ErrorResponseHelper<string>("Error while attempting to toggle status");

      return _responseHelper.SuccessResponseHelper<string>("Status toggled successfully", null);
    }
  }
}