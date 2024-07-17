using ecommerce.DTO;
using ecommerce.Helpers;
using ecommerce.Models;
using ecommerce.Services;
using ecommerce.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
  [ApiController]
  [Route("/api/auth")]
  public class AuthController : ControllerBase
  {
    private UserRepository _userRepository;
    private ResponseHelper _responseHelper;

    public AuthController(UserRepository userRepository, ResponseHelper responseHelper)
    {
      _userRepository = userRepository;
      _responseHelper = responseHelper;
    }

    [HttpPost("/register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateUserDTO))]
    public IActionResult RegisterUser([FromBody]CreateUserDTO createUserDTO, IFormFile avatar)
    {
      string avatarPath = null;
      if (avatar != null && avatar.Length > 0)
      {
          // You can save the file to the server or process it as needed
          var filePath = Path.Combine("uploads", avatar.FileName);
          using (var stream = new FileStream(filePath, FileMode.Create))
          {
              avatar.CopyToAsync(stream);
          }
          avatarPath = filePath; // Set the path or URL to the uploaded file
          // Console.ReadLine(avatarPath);
      }

      // if request body is empty
      if (createUserDTO == null) return _responseHelper.ErrorResponseHelper<string>("Request body cannot be empty", null, 422);

      // check if email exist
      if (_userRepository.CheckIfEmailExists(createUserDTO.Email))
        return _responseHelper.ErrorResponseHelper<string>($"User with email {createUserDTO.Email.ToLower()} already exist");

      // check if model state is valid
      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState);

      var newUser = _userRepository.Register(createUserDTO);

      // check if it has saved
      if (!newUser) return _responseHelper.ErrorResponseHelper("Something went wrong while attempting to save your record", ModelState, 500);

      return _responseHelper.SuccessResponseHelper("User created successfully", newUser, 201);
    }

    [HttpPost("/login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserLoginDTO))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult LoginUser([FromBody] UserLoginDTO userLoginDTO)
    {
       // if request body is empty
      if (userLoginDTO == null) return _responseHelper.ErrorResponseHelper<string>("Request body cannot be empty", null, 422);

      // check if email exist
      if (!_userRepository.CheckIfEmailExists(userLoginDTO.Email))
        return _responseHelper.ErrorResponseHelper<string>($"User with email {userLoginDTO.Email.ToLower()} does not exist");

      // check if model state is valid
      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState);

      // check login credentials
      var checkLoginCredentials = _userRepository.LoginUser(userLoginDTO);

      if (checkLoginCredentials == null) return _responseHelper.ErrorResponseHelper<string>("Login or Email is incorrect");

      return _responseHelper.SuccessResponseHelper("User token retrieved successfully", checkLoginCredentials);
    }
  }
}