using ToDo.Data.Entities;

namespace ToDo.Data.Interfaces
{
    /// <summary>
    /// Interface for managing user Repository
    /// </summary>
    public interface IUserRepo
    {
        /// <summary>
        /// Check if the user is valid
        /// </summary>
        /// <param name="user">The user details to validate.</param>
        /// <returns>The logged in user.</returns>
        public Task<User> IsValidUserAsync(User user);

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user">The details of user.</param>
        /// <returns>True if the user is created successfully, false otherwise.</returns>
        public Task<bool> CreateUserAsync(User user);
        
        /// <summary>
        /// Check if the user is existed
        /// </summary>
        /// <param name="userName">The name of the user</param>
        /// <returns>True if the user is existed, false otherwise.</returns>
        public Task<bool> IsUserExistedAsync(string userName);
    }
}
