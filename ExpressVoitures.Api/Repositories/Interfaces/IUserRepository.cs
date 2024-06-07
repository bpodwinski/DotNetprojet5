using ExpressVoituresApi.Models.Entities;

namespace ExpressVoituresApi.Repositories.Interfaces
{
    /// <summary>
    /// Interface for user repository.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets a user by its ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The user entity.</returns>
        Task<User?> GetById(int id);

        /// <summary>
        /// Gets a user by its email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>The user entity.</returns>
        Task<User> GetByEmail(string email);

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="user">The user entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Add(User user);

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="user">The user entity to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Update(User user);

        /// <summary>
        /// Updates the token of an existing user.
        /// </summary>
        /// <param name="user">The user entity to update the token for.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateToken(User user);
    }
}
