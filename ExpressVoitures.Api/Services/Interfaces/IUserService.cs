using ExpressVoituresApi.Models.Entities;

namespace ExpressVoituresApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> Authenticate(string email, string password);
        Task<UserDto> CreateUser(UserDto userDto);
        Task<UserDto> GetUserById(int id);
    }
}