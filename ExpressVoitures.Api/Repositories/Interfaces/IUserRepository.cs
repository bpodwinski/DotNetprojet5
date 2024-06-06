using ExpressVoituresApi.Models.Entities;

namespace ExpressVoituresApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string email);
        Task AddUser(User user);
        Task UpdateUser(User user);
        Task UpdateUserToken(User user);
    }
}