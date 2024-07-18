using ToDo.Data.Entities;

namespace ToDo.Data.Interfaces
{
    /// <summary>
    /// Interface for managing todo items Repository
    /// </summary>
    public interface IToDoItemRepo
    {
        /// <summary>
        /// Adds a new task
        /// </summary>
        /// <param name="newItem">The new task to added.</param>
        /// <returns>The added task .</returns>
        public Task<ToDoItem> AddTaskAsync(ToDoItem newItem);

        /// <summary>
        /// Removes a task by its ID.
        /// </summary>
        /// <param name="taskId">The id of the task.</param>
        /// <param name="userId">The id of the user</param>
        /// <returns> The added task.</returns>
        public Task<ToDoItem> RemoveTaskAsync(int taskId, int userId);

        /// <summary>
        /// Updates a task.
        /// </summary>
        /// <param name="item">The task to be updated.</param>
        /// <returns>The updated task.</returns>
        public Task<ToDoItem> UpdateTaskAsync(ToDoItem item);

        /// <summary>
        /// Removes all tasks by status and date of that user.
        /// </summary>
        /// <param name="userId">The Id of the user.</param>
        /// <param name="status">The status of the task.</param>
        /// <param name="date">The date of the task.</param>
        /// <returns>True if the tasks are removed successfully, false otherwise.</returns>
        public Task<bool> RemoveAllTasksAsync(int userId, string status, DateTime date);

        /// <summary>
        /// Gets a task by its ID of that user.
        /// </summary>
        /// <param name="taskId">The id of the task.</param>
        /// <param name="userId">The id of the user.</param>
        /// <returns>The task if it exists, null otherwise.</returns>
        public Task<ToDoItem> GetTaskByIdAsync(int taskId, int userId);

        /// <summary>
        /// Gets all tasks by status and date of that user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="status">The status of the task.</param>
        /// <param name="date">The date of the task.</param>
        /// <returns>The list of tasks if they exist, an empty list otherwise.</returns>
        public Task<List<ToDoItem>> GetAllTasksAsync(int userId, string status, DateTime date);

        /// <summary>
        /// Gets all tasks by status of that user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="status">The status of the task.</param>
        /// <returns>The list of tasks.</returns>
        public Task<List<ToDoItem>> GetTasksByStatusAsync(int userId, bool status);
    }
}
