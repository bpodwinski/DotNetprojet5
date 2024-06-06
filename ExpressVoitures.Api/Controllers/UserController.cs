using Microsoft.AspNetCore.Mvc;
using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Services.Interfaces;
using ExpressVoituresApi.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Newtonsoft.Json.Linq;

namespace ExpressVoituresApi.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService userService,
            IAuthService authService,
            ILogger<UserController> logger
            )
        {
            _userService = userService;
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user based on email and password
        /// </summary>
        /// <param name="userLoginDto">The user login data transfer object containing the user's email and password.</param>
        /// <returns>An IActionResult that may contain a JWT token if authentication is successful, a bad request response if input is invalid, or an unauthorized response if authentication fails.</returns>
        /// <remarks>
        /// This method allows anonymous access and is typically used to handle the initial login process for a user.
        /// It checks if the provided userLoginDto is null, authenticates the user, and generates a JWT token for authenticated users.
        /// </remarks>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            try
            {
                if (userLoginDto == null)
                {
                    _logger.LogWarning("UserLoginDto is null");
                    return BadRequest(new { Message = "User login data is required" });
                }

                var user = await _authService.Authenticate(userLoginDto.email, userLoginDto.password);

                if (user == null)
                {
                    return Unauthorized(new { Message = "Invalid email or password" });
                }

                await _userService.UpdateUserToken(user.id);

                return Ok(await _userService.GetUserById(user.id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in the user");
                return StatusCode(500, new { Message = "An error occurred while logging in the user" });
            }
        }

        /// <summary>
        /// Creates a new user account
        /// </summary>
        /// <param name="userDto">The user data transfer object.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        /// <response code="201">User created successfully.</response>
        /// <response code="400">If the request parameters are invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserCreateDto userCreateDto)
        {
            try
            {
                if (userCreateDto == null)
                {
                    _logger.LogWarning("userCreateDto is null");
                    return BadRequest(new { Message = "User data is required" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userService.CreateUser(userCreateDto);

                await _userService.UpdateUserToken(user.id);
                
                return Ok(await _userService.GetUserById(user.id));
            }
            catch (EmailExistsException)
            {
                _logger.LogWarning("Attempt to create a user with an existing email: {Email}", userCreateDto.email);
                return Conflict(new { Message = "Email already exists" });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An error occurred while creating the user account");
                return StatusCode(500, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating the user account");
                return StatusCode(500, new { Message = "An unexpected error occurred while creating the user account" });
            }
        }

        /// <summary>
        /// Updates a user account
        /// </summary>
        /// <param name="userDto">The user data transfer object.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        /// <response code="201">User created successfully.</response>
        /// <response code="400">If the request parameters are invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto userUpdateDto)
        {
            if (userUpdateDto == null || id != userUpdateDto.id)
            {
                return BadRequest(new { Message = "Invalid user data" });
            }

            try
            {
                var updatedUser = await _userService.UpdateUser(userUpdateDto);
                return Ok(updatedUser);
            }
            catch (EmailExistsException)
            {
                return Conflict(new { Message = "Email already exists" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user");
                return StatusCode(500, new { Message = "An error occurred while updating the user" });
            }
        }

        /// <summary>
        /// Refreshes the JWT token
        /// </summary>
        /// <param name="refreshTokenDto">The refresh token data transfer object containing the token and refresh token.</param>
        /// <returns>An IActionResult that may contain a new JWT token if refresh is successful, or an unauthorized response if refresh fails.</returns>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDto tokenDto)
        {
            try
            {
                if (tokenDto == null || string.IsNullOrEmpty(tokenDto.token) || string.IsNullOrEmpty(tokenDto.refresh_token))
                {
                    _logger.LogWarning("RefreshTokenDto is invalid");
                    return BadRequest(new { Message = "Invalid refresh token data" });
                }

                var principal = _authService.GetPrincipalFromExpiredToken(tokenDto.token);
                if (principal == null)
                {
                    return Unauthorized(new { Message = "Invalid token" });
                }

                var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var user = await _userService.GetUserById(userId);
                if (user == null || user.refresh_token != tokenDto.refresh_token || user.refresh_token_expiry_time <= DateTime.Now)
                {
                    return Unauthorized(new { Message = "Invalid refresh token" });
                }

                return Ok(await _userService.UpdateUserToken(user.id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while refreshing the token");
                return StatusCode(500, new { Message = "An error occurred while refreshing the token" });
            }
        }

        /// <summary>
        /// Retrieves a user by ID
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The user with the specified ID.</returns>
        /// <response code="200">Returns the user with the specified ID.</response>
        /// <response code="400">If the request parameters are invalid.</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound(new { Message = $"User with ID {id} not found" });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving user with ID {id}");
                return StatusCode(500, new { Message = "An error occurred while retrieving the user" });
            }
        }
    }
}
