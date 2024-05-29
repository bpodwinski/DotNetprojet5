using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExpressVoituresApi.Models.Entities;
using Microsoft.IdentityModel.Tokens;

/// <summary>
/// Service to generate JWT tokens for authenticated users.
/// </summary>
public class TokenService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the TokenService with the specified configuration.
    /// </summary>
    /// <param name="configuration">Configuration interface provided by the application.</param>
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Generates a JWT token for the specified User DTO.
    /// </summary>
    /// <param name="userDto">The user data transfer object containing the user's details.</param>
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
            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to generate token", ex);
        }
    }
}