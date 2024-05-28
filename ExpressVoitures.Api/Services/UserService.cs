using ExpressVoituresApi.Data;
using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Repositories.Interfaces;
using ExpressVoituresApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ExpressVoituresApi.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<UserService> _logger;

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

        public async Task<bool> EmailExist(string email)
        {
            var user = await _userRepository.GetByEmail(email);
            return user != null;
        }

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
                throw new InvalidOperationException("An error occurred while authenticating the user", ex);
            }
        }

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

        public async Task<UserDto> GetUserById(int id)
        {
            var userAccount = await _context.Users.FindAsync(id);
            if (userAccount == null)
            {
                throw new InvalidOperationException($"UserAccount with ID {id} not found");
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
    }
}