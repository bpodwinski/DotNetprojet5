using ExpressVoituresApi.Models.Entities;

namespace ExpressVoituresApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUser(UserCreateDto userAddDto);
        Task<UserUpdateDto> UpdateUser(UserUpdateDto userUpdateDto);
        Task<TokenDto> UpdateUserToken(int id);
        Task<UserDto> GetUserById(int id);
        Task<bool> EmailExist(string email);
    }
}