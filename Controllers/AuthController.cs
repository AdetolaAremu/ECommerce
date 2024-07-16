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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateUserDTO))]
    public IActionResult RegisterUser([FromBody]CreateUserDTO createUserDTO)
    {
      // if request body is empty
      if (createUserDTO == null) return _responseHelper.ErrorResponseHelper<string>("Request body cannot be empty", null, 422);

      // check if email exist
      if (!_userRepository.CheckIfEmailExists(createUserDTO.Email))
        return _responseHelper.ErrorResponseHelper<string>($"User with email {createUserDTO.Email.ToLower()} already exist");

      // check if model state is valid
      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper("An error occurred", ModelState);

      var newUser = _userRepository.Register(createUserDTO);

      // check if it has saved
      if (!newUser) return _responseHelper.ErrorResponseHelper("Something went wrong while attempting to save your record", ModelState, 500);

      return _responseHelper.SuccessResponseHelper("User created successfully", newUser, 201);
    }
  }
}