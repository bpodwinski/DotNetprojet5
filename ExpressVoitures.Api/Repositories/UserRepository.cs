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

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.email == email);
        }

        public async Task Add(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}