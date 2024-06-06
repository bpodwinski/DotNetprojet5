using ExpressVoituresApi.Data;
using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoituresApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserToken(User user)
        {
            var _user = await _context.Users.FindAsync(user.id);

            _user.refresh_token = user.refresh_token;
            _user.refresh_token_expiry_time = user.refresh_token_expiry_time;

            _context.Users.Update(_user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.email == email);

            return user;
        }
    }
}