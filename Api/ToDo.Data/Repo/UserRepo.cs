using ToDo.Data.DBContext;
using ToDo.Data.Entities;
using ToDo.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Data.Repo
{
    /// <inheritdoc cref="IUserRepo"/>
    public class UserRepo : IUserRepo
    {
        private ToDoDBContext _dbContext;

        /// <summary>
        /// Constructor for <see cref="UserRepo"/>
        /// </summary>
        /// <param name="dbContext">The instance of <see cref="ToDoDBContext"/> to be used.</param>
        public UserRepo(ToDoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateUserAsync(User newUser)
        {
            await _dbContext.Users.AddAsync(newUser);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsUserExistedAsync(string userName)
        {
            // return await _dbContext.Users.Where( u => EF.Functions.Collate(u.Name, "SQL_Latin1_General_CP1_CS_AS") == userName).AnyAsync(); 
            return await _dbContext.Users.AnyAsync(u => u.Name == userName);
        }

        public async Task<User> IsValidUserAsync(User user)
        {
            var loggedInUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Name == user.Name);

            if (loggedInUser != null && BCrypt.Net.BCrypt.Verify(user.Password, loggedInUser.Password))
            {
                return loggedInUser;
            }
            return null!;
        }
    }
}
