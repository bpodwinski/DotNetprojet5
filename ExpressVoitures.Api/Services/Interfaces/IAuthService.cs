using ExpressVoituresApi.Models.Entities;
using System.Security.Claims;

namespace ExpressVoituresApi.Services.Interfaces
{
    /// <summary>
    /// Interface defining the authentication methods for the authentication service.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user using their email and password.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>A <see cref="UserCreateDto"/> object representing the authenticated user.</returns>
        Task<UserCreateDto> Authenticate(string email, string password);

        /// <summary>
        /// Generates a JWT token for a given user.
        /// </summary>
        /// <param name="userDto">The <see cref="UserDto"/> object representing the user.</param>
        /// <returns>A string representing the generated JWT token.</returns>
        string GenerateToken(UserDto userDto);

        /// <summary>
        /// Generates a refresh token for a given user.
        /// </summary>
        /// <param name="userDto">The <see cref="UserDto"/> object representing the user.</param>
        /// <returns>A string representing the generated refresh token.</returns>
        string GenerateRefreshToken(UserDto userDto);

        /// <summary>
        /// Gets the principal from an expired JWT token.
        /// </summary>
        /// <param name="token">The expired JWT token.</param>
        /// <returns>A <see cref="ClaimsPrincipal"/> object representing the user, or null if the token is invalid.</returns>
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}