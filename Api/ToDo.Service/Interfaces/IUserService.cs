using ToDo.Service.DTO;
using ToDo.Service.DTO.User;

namespace ToDo.Service.Interfaces
{
    /// <summary>
    /// Interface for managing user Service
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Check if the user is existed
        /// </summary>
        /// <param name="userName">The name of the user.</param>
        /// <returns>status of the user existence.</returns>
        public Task<bool> IsUserExisted(string userName);

        // public Task<bool> GetUserByName(string userName);

        /// <summary>
        /// Login the user
        /// </summary>
        /// <param name="user">The user details.</param>
        /// <returns>The logged in user.</returns>
        public Task<UserDetails> Login(LoginRegisterModel user);

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="newUser">The new user details.</param>
        /// <returns>The status of the registration.</returns>
        public Task<bool> Register(LoginRegisterModel newUser);

        /// <summary>
        /// To get the user id
        /// </summary>
        /// <returns>Returns the user id from claims</returns>
        public int GetUserId();
    }
}
