using ExpressVoituresApi.Data;
using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Repositories.Interfaces;
using ExpressVoituresApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ExpressVoituresApi.Services
{
    /// <summary>
    /// Provides services for user management, including operations such as creation, deletion, and updates of user information.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepository;
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
            ILogger<UserService> logger
            )
        {
            _context = context;
            _userRepository = userRepository;
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
                var user = await _userRepository.GetByEmail(email);
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
        /// <param name="userDto">The user data transfer object containing the user's details for creation.</param>
        /// <returns>A newly created UserDto with ID and creation date filled.</returns>
        /// <exception cref="EmailExistsException">Thrown when an attempt is made to create a user with an email that already exists in the database.</exception>
        /// <exception cref="Exception">General exceptions are caught and rethrown, indicating an unexpected error occurred during the operation.</exception>
        public async Task<UserDto> Authenticate(string email, string password)
        {
            try
            {
                var user = await _userRepository.GetByEmail(email);
                if (user == null || _passwordHasher.VerifyHashedPassword(user, user.password, password) == PasswordVerificationResult.Failed)
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
                    password = user.password
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while authenticating the user");
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
        public async Task<UserDto> CreateUser(UserDto userDto)
        {
            try
            {
                if (await EmailExist(userDto.email))
                {
                    _logger.LogWarning("Attempt to create a user with an existing email: {Email}", userDto.email);
                    throw new EmailExistsException();
                }
                
                var user = new User
                {
                    firstname = userDto.firstname,
                    lastname = userDto.lastname,
                    email = userDto.email,
                    password = userDto.password
                };

                user.password = _passwordHasher.HashPassword(user, userDto.password);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                userDto.id = user.id;
                userDto.create_date = user.create_date;

                return userDto;
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

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier for the user.</param>
        /// <returns>A data transfer object representing the user.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no user is found with the provided ID.</exception>
        public async Task<UserDto> GetUserById(int id)
        {
            try
            {
                var userAccount = await _context.Users.FindAsync(id);

                if (userAccount == null)
                {
                    throw new InvalidOperationException($"User with ID {id} not found");
                }

                return new UserDto
                {
                    id = userAccount.id,
                    create_date = userAccount.create_date,
                    firstname = userAccount.firstname,
                    lastname = userAccount.lastname,
                    email = userAccount.email,
                    password = userAccount.password
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