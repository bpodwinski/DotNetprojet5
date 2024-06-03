using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Repositories.Interfaces;
using ExpressVoituresApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ExpressVoituresApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IConfiguration configuration,
            IUserRepository userRepository,
            ILogger<AuthService> logger
            )
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
            _logger = logger;
        }

        /// <summary>
        /// Creates a new user in the database.
        /// </summary>
        /// <param name="UserCreateDto">The user data transfer object containing the user's details for creation.</param>
        /// <returns>A newly created UserDto with ID and creation date filled.</returns>
        /// <exception cref="EmailExistsException">Thrown when an attempt is made to create a user with an email that already exists in the database.</exception>
        /// <exception cref="Exception">General exceptions are caught and rethrown, indicating an unexpected error occurred during the operation.</exception>
        public async Task<UserCreateDto> Authenticate(string email, string password)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(email);

                if (user == null || _passwordHasher.VerifyHashedPassword(user, user.password, password) == PasswordVerificationResult.Failed)
                {
                    return null;
                }

                return new UserCreateDto
                {
                    id = user.id,
                    create_date = user.create_date,
                    firstname = user.firstname,
                    lastname = user.lastname,
                    email = user.email,
                    password = user.password,
                    token = user.token,
                    refresh_token = user.refresh_token,
                    refresh_token_expiry_time = user.refresh_token_expiry_time
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while authenticating the user");
                throw;
            }
        }

        /// <summary>
        /// Generates a JWT token for the specified User DTO.
        /// </summary>
        /// <param name="userTokenUpdateDtoserDto">The user data transfer object containing the user's details.</param>
        /// <returns>A JWT token as a string.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the token cannot be generated.</exception>
        public string GenerateToken(UserDto userDto)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

                // Define the token descriptor based on user details and application settings.
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, userDto.id.ToString()),
                        new Claim(ClaimTypes.Email, userDto.email)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                //userDto.refresh_token = refreshToken;
                //userDto.refresh_token_expiry_time = DateTime.UtcNow.AddDays(1);

                //_userService.UpdateUserToken(new UserTokenUpdateDto
                //{
                //    id = userDto.id,
                //    token = token,
                //    refresh_token = userDto.refresh_token,
                //    refresh_token_expiry_time = userDto.refresh_token_expiry_time
                //}).Wait();

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to generate token", ex);
            }
        }

        public string GenerateRefreshToken(UserDto userDto)
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                var refreshToken = Convert.ToBase64String(randomNumber);

                userDto.refresh_token = refreshToken;
                userDto.refresh_token_expiry_time = DateTime.UtcNow.AddDays(1);

                //_userService.UpdateUserToken(new TokenDto
                //{
                //    id = userDto.id,
                //    token = userDto.token,
                //    refresh_token = userDto.refresh_token,
                //    refresh_token_expiry_time = userDto.refresh_token_expiry_time
                //}).Wait();

                return refreshToken;
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, // You might want to validate the audience and issuer in your actual application
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateLifetime = false // Here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
