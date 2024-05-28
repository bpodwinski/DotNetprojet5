using ExpressVoituresApi.Models.Entities;

namespace ExpressVoituresApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByEmail(string email);
        Task Add(User user);
        Task<User> GetById(int id);
    }
}