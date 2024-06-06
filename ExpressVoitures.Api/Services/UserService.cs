using ExpressVoituresApi.Data;
using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Repositories.Interfaces;
using ExpressVoituresApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace ExpressVoituresApi.Services
{
    /// <summary>
    /// Provides services for user management, including operations such as creation, deletion, and updates of user information.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<UserService> _logger;

        /// <summary>
        /// Initializes a new instance of the UserService.
        /// </summary>
        /// <param name="context">The database context used for data operations.</param>
        /// <param name="userRepository">The repository handling user data operations.</param>
        /// <param name="logger">Logger for capturing runtime information and errors.</param>
        public UserService(
            ApplicationDbContext context,
            IUserRepository userRepository,
            IAuthService authService,
            ILogger<UserService> logger
            )
        {
            _context = context;
            _userRepository = userRepository;
            _authService = authService;
            _passwordHasher = new PasswordHasher<User>();
            _logger = logger;
        }

        /// <summary>
        /// Checks if an email already exists in the user repository.
        /// </summary>
        /// <param name="email">The email address to check in the user repository.</param>
        /// <returns>True if the email exists; otherwise, false.</returns>
        /// <exception cref="ArgumentException">Thrown when the provided email is null or whitespace.</exception>
        /// <exception cref="Exception">Thrown if there is an issue accessing the user data.</exception>
        public async Task<bool> EmailExist(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email parameter cannot be null or whitespace.", nameof(email));
            }

            try
            {
                var user = await _userRepository.GetUserByEmail(email);
                return user != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while checking if the email '{email}' exists.");
                throw;
            }
        }

        /// <summary>
        /// Creates a new user in the database.
        /// </summary>
        /// <param name="userDto">Data transfer object containing user details.</param>
        /// <returns>The created UserDto with ID and creation date set.</returns>
        /// <exception cref="EmailExistsException">Thrown when an attempt is made to create a user with an email that already exists.</exception>
        /// <exception cref="Exception">General exceptions are caught and logged, and then rethrown.</exception>
        public async Task<UserDto> CreateUser(UserCreateDto userCreateDto)
        {
            try
            {
                if (await EmailExist(userCreateDto.email))
                {
                    _logger.LogWarning("Attempt to create a user with an existing email: {Email}", userCreateDto.email);
                    throw new EmailExistsException();
                }
                
                var user = new User
                {
                    firstname = userCreateDto.firstname,
                    lastname = userCreateDto.lastname,
                    email = userCreateDto.email,
                    password = userCreateDto.password,
                };

                user.password = _passwordHasher.HashPassword(user, userCreateDto.password);

                await _userRepository.AddUser(user);

                return new UserDto
                {
                    id = user.id,
                    create_date = user.create_date,
                    firstname = user.firstname,
                    lastname = user.lastname,
                    email = user.email,
                    password = user.password,
                    refresh_token = user.refresh_token,
                    refresh_token_expiry_time = user.refresh_token_expiry_time
                };
            }
            catch (EmailExistsException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the user account");
                throw;
            }
        }

        public async Task<UserUpdateDto> UpdateUser(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userRepository.GetUserById(userUpdateDto.id);

                if (user == null)
                {
                    throw new InvalidOperationException($"User with ID {userUpdateDto.id} not found");
                }

                if (user.email != userUpdateDto.email && await EmailExist(userUpdateDto.email))
                {
                    _logger.LogWarning("Attempt to update a user with an existing email: {Email}", userUpdateDto.email);
                    throw new EmailExistsException();
                }

                user.id = userUpdateDto.id;
                user.firstname = userUpdateDto.firstname;
                user.lastname = userUpdateDto.lastname;
                user.email = userUpdateDto.email;
                user.token = userUpdateDto.token;
                user.refresh_token = userUpdateDto.refresh_token;

                await _userRepository.UpdateUser(user);

                return userUpdateDto;
            }
            catch (EmailExistsException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user account");
                throw;
            }
        }

        public async Task<TokenDto> UpdateUserToken(int id)
        {
            var user = await _userRepository.GetUserById(id);

            if (user == null)
            {
                throw new InvalidOperationException($"User with ID {id} not found");
            }

            var userDto = new UserDto
            {
                id = user.id,
                email = user.email
            };

            var token = _authService.GenerateToken(userDto);
            var refreshToken = _authService.GenerateRefreshToken(userDto);
            var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);

            user.token = token;
            user.refresh_token = refreshToken;
            user.refresh_token_expiry_time = refreshTokenExpiryTime;

            await _userRepository.UpdateUser(user);

            var tokenDto = new TokenDto
            {
                id = id,
                token = token,
                refresh_token = refreshToken,
                refresh_token_expiry_time = refreshTokenExpiryTime
            };

            return tokenDto;
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier for the user.</param>
        /// <returns>A data transfer object representing the user.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no user is found with the provided ID.</exception>
        public async Task<UserDto?> GetUserById(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return null;
                }

                return new UserDto
                {
                    id = user.id,
                    create_date = user.create_date,
                    firstname = user.firstname,
                    lastname = user.lastname,
                    email = user.email,
                    password = user.password,
                    token = user.token,
                    refresh_token = user.refresh_token,
                    refresh_token_expiry_time = user.refresh_token_expiry_time,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve user with ID {UserId}", id);
                throw;
            }
        }
    }
}