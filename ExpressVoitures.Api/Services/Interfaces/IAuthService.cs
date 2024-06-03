using ExpressVoituresApi.Models.Entities;
using System.Security.Claims;

namespace ExpressVoituresApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserCreateDto> Authenticate(string email, string password);
        string GenerateToken(UserDto userDto);
        string GenerateRefreshToken(UserDto userDto);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
