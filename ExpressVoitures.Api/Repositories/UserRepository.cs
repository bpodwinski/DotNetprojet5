using ExpressVoituresApi.Data;
using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoituresApi.Repositories
{
    /// <summary>
    /// Repository for managing user entities.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="user">The user entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Add(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="user">The user entity to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the token of an existing user.
        /// </summary>
        /// <param name="user">The user entity to update the token for.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateToken(User user)
        {
            var _user = await _context.Users.FindAsync(user.id);

            _user.refresh_token = user.refresh_token;
            _user.refresh_token_expiry_time = user.refresh_token_expiry_time;

            _context.Users.Update(_user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets a user by its ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The user entity.</returns>
        public async Task<User?> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        /// <summary>
        /// Gets a user by its email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>The user entity.</returns>
        public async Task<User?> GetByEmail(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.email == email);
        }
    }
}
