using ExpressVoituresApi.Models.Entities;

namespace ExpressVoituresApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUser(UserCreateDto userAddDto);
        Task<UserUpdateDto> UpdateUser(UserUpdateDto userUpdateDto);
        Task UpdateUserToken(TokenDto tokenDto);
        Task<UserDto> GetUserById(int id);
        Task<bool> EmailExist(string email);
    }
}