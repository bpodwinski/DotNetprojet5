using Microsoft.AspNetCore.Mvc;
using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ExpressVoituresApi.Models.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace ExpressVoituresApi.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenService _tokenService;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService userService,
            TokenService tokenService,
            ILogger<UserController> logger
            )
        {
            _userService = userService;
            _tokenService = tokenService;
            _logger = logger;
        }

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

                var userDto = await _userService.Authenticate(userLoginDto.email, userLoginDto.password);
                if (userDto == null)
                {
                    return Unauthorized(new { Message = "Invalid email or password" });
                }

                var token = _tokenService.GenerateToken(userDto);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in the user");
                return StatusCode(500, new { Message = "An error occurred while logging in the user" });
            }
        }


        /// <summary>
        /// Creates a new user account.
        /// </summary>
        /// <param name="userDto">The user data transfer object.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        /// <response code="201">User created successfully.</response>
        /// <response code="400">If the request parameters are invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            try
            {
                if (userDto == null)
                {
                    _logger.LogWarning("UserDto is null");
                    return BadRequest(new { Message = "User data is required" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                } 

                await _userService.CreateUser(userDto);
                return StatusCode(201);
            }
            catch (EmailExistsException)
            {
                _logger.LogWarning("Attempt to create a user with an existing email: {Email}", userDto.email);
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
        /// Retrieves a user by ID.
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
