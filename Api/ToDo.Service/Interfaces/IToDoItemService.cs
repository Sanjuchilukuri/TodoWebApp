using Microsoft.AspNetCore.JsonPatch;
using ToDo.Service.DTO.ToDoItem;

namespace ToDo.Service.Interfaces
{
    /// <summary>
    /// Interface for managing todo items Service
    /// </summary>
    public interface IToDoItemService
    {
        /// <summary>
        /// Adds a new task
        /// </summary>
        /// <param name="newItem">The new item to add.</param>
        /// <returns>The added item.</returns>
        public Task<ToDoItemDetail> AddTask(ToDoItem newItem);

        /// <summary>
        /// Updates a task by its ID.
        /// </summary>
        /// <param name="Item">The item to Update.</param>
        /// <param name="taskId">The Id of the task to update.</param>
        /// <returns>The Updated Task</returns>
        public Task<ToDoItemDetail> UpdateTask(ToDoItem Item, int taskId);

        /// <summary>
        /// Deletes a task by its ID.
        /// </summary>
        /// <param name="taskId">The Id of the task to delete.</param>
        /// <returns>The deleted task.</returns>
        public Task<ToDoItemDetail> DeleteTask(int taskId, int userId);

        /// <summary>
        /// Gets a task by its ID.
        /// </summary>
        /// <param name="taskId">The Id of the task to filter.</param>
        /// <returns>The filter Task by Id</returns>
        public Task<ToDoItemDetail> GetTaskById(int taskId, int userId);

        /// <summary>
        /// Removes all tasks that match the filters.
        /// </summary>
        /// <param name="filters">The filters need to apply.</param>
        /// <returns>The status of delete operation</returns>
        public Task<bool> RemoveAllTasks(Filters filters, int userId);

        /// <summary>
        /// Gets all tasks that match the filters.
        /// </summary>
        /// <param name="filters">The filters need to apply.</param>
        /// <returns>The list of tasks</returns>
        public Task<List<ToDoItemSummary>> GetAllTasks(Filters filters, int userId);

        /// <summary>
        /// Gets all tasks that match the status.
        /// </summary>
        /// <param name="status">The status of the task.</param>
        /// <returns>The list of tasks.</returns>
        public Task<List<ToDoItemDetail>> GetTasksByStatus(bool status, int userId);

        /// <summary>
        /// Marks a task as completed.
        /// </summary>
        /// <param name="taskId">The Id of the task.</param>
        /// <param name="patchDocument">The patch document containing the updates.</param>
        /// <returns>The updated task.</returns>
        public Task<ToDoItemDetail> TaskCompleted(int taskId, JsonPatchDocument patchDocument, int userId);

    }
}
