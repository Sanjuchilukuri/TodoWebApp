using ToDo.Data.DBContext;
using ToDo.Data.Entities;
using ToDo.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Data.Repo
{
    /// <inheritdoc cref="IToDoItemRepo"/>
    public class ToDoItemRepo : IToDoItemRepo
    {
        private readonly ToDoDBContext _dbContext;
        
        /// <summary>
        /// Constructor for <see cref="ToDoItemRepo"/>
        /// </summary>
        /// <param name="dbContext">The instance of <see cref="ToDoDBContext"/> to be used.</param>
        public ToDoItemRepo(ToDoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IQueryable<ToDoItem> ApplyUserFilter(IQueryable<ToDoItem> query, int userId)
        {
            return query.Where(task => task.UserId == userId);
        }

        private IQueryable<ToDoItem> ApplyDateFilter(IQueryable<ToDoItem> query, DateTime date)
        {
            var targetDate = DateOnly.FromDateTime(date);
            return query.Where(task => DateOnly.FromDateTime(task.CreatedOn) == targetDate);
        }

        private IQueryable<ToDoItem> ApplyCompletionFilter(IQueryable<ToDoItem> query, bool isCompleted)
        {
            return query.Where(task => task.IsCompleted == isCompleted);
        }

        public async Task<ToDoItem> AddTaskAsync(ToDoItem newItem)
        {
            await _dbContext.Tasks.AddAsync(newItem);
            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return newItem;
            }
            return null!;
        }

        public async Task<List<ToDoItem>> GetAllTasksAsync(int userId, string status, DateTime date)
        {
            var query = _dbContext.Tasks.AsQueryable();
            query = ApplyUserFilter(query, userId);
            query = ApplyDateFilter(query, date);
            if (status == "completed")
            {
                query = ApplyCompletionFilter(query, true);
            }
            else if (status == "active")
            {
                query = ApplyCompletionFilter(query, false);
            }
            return await query.OrderBy(task => task.IsCompleted).ToListAsync();
        }

        public async Task<ToDoItem> GetTaskByIdAsync(int taskId, int userId)
        {
            var query = _dbContext.Tasks.AsQueryable();
            query = ApplyUserFilter(query, userId);
            var task = await query.SingleOrDefaultAsync(task => task.Id == taskId);
            return task!;
        }

        public async Task<bool> RemoveAllTasksAsync(int userId, string status, DateTime date)
        {
            var query = _dbContext.Tasks.AsQueryable();
            query = ApplyUserFilter(query, userId);
            query = ApplyDateFilter(query, date);
            if (status == "completed")
            {
                query = ApplyCompletionFilter(query, true);
            }
            else if (status == "active")
            {
                query = ApplyCompletionFilter(query, false);
            }
            return await query.ExecuteDeleteAsync() > 0;
        }

        public async Task<ToDoItem> RemoveTaskAsync(int taskId, int userId)
        {
            var query = _dbContext.Tasks.AsQueryable();
            query = ApplyUserFilter(query, userId);
            var deletedTask = await query.SingleOrDefaultAsync(task => task.Id == taskId);

            if (deletedTask != null)
            {
                _dbContext.Tasks.Remove(deletedTask);
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return deletedTask;
                }
            }
            return null!;
        }

        public async Task<ToDoItem> UpdateTaskAsync(ToDoItem item)
        {
            
            var existingItem = await _dbContext.Tasks.FindAsync(item.Id);

            item.CreatedOn = existingItem!.CreatedOn;
            item.CreatedBy = existingItem.CreatedBy;

            _dbContext?.Entry(existingItem!).CurrentValues.SetValues(item);

            _dbContext!.Entry(existingItem).Property(e => e.CreatedOn).IsModified = false;
            _dbContext.Entry(existingItem).Property(e => e.CreatedBy).IsModified = false;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return existingItem!;
            }
            return null!;
        }

        public async Task<List<ToDoItem>> GetTasksByStatusAsync(int userId, bool status)
        {
            var query = _dbContext.Tasks.AsQueryable();
            query = ApplyUserFilter(query, userId);
            query = ApplyCompletionFilter(query, status);
            return await query.ToListAsync();
        }
    }
}
