using ToDo.Service.DTO.ToDoItem;
using ToDo.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ToDo.WebApi.Controllers
{
    /// <summary>
    /// Controller for managing ToDo items.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ToDoItemController : ControllerBase
    {
        private IToDoItemService _toDoService;
        private IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemController"/> interface.
        /// </summary>
        /// <param name="toDoServices">Service for managing ToDo items.</param>
        public ToDoItemController(IToDoItemService toDoService, IUserService userService)
        {
            _toDoService = toDoService;
            _userService = userService;
        }

        /// <summary>
        /// Adds a new ToDo task.
        /// </summary>
        /// <param name="newTask">The new task to add.</param>
        /// <returns>The added task.</returns>
        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] ToDoItem newTask)
        {
            newTask.UserId = _userService.GetUserId();
            var addedTask = await _toDoService.AddTask(newTask);

            if (addedTask != null)
            {
                return Created(nameof(GetTaskById), addedTask);
            }

            throw new Exception("Failed to add task");
        }


        /// <summary>
        /// Gets a task by its ID.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <returns>The task with the specified ID.</returns>
        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTaskById([FromRoute] int taskId)
        {
            int userId = _userService.GetUserId();
            var task = await _toDoService.GetTaskById(taskId,userId);
            if (task != null)
            {
                return Ok(task);
            }
            else
            {
                return BadRequest($"Task not found with id : {taskId}");
            }
        }

        /// <summary>
        /// Gets all tasks based on the specified filters.
        /// </summary>
        /// <param name="filters">The filters to apply.</param>
        /// <returns>A list of tasks that match the filters.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllTasks([FromQuery] Filters filters)
        {
            int userId = _userService.GetUserId();
            var allTasks = await _toDoService.GetAllTasks(filters,userId);

            if (allTasks.Count == 0)
            {
                return NotFound("No Tasks Found");
            }
            else
            {
                return Ok(allTasks);
            }
        }

        /// <summary>
        /// Delete a task by its ID.
        /// </summary>
        /// <param name="taskId">The taskId to delete.</param>
        /// <returns>A tasks that was deleted.</returns>
        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTaskById([FromRoute] int taskId)
        {
            int userId = _userService.GetUserId();
            var deletedTask = await _toDoService.DeleteTask(taskId,userId);
            if (deletedTask != null)
            {
                return Ok(deletedTask);
            }
            else
            {
                return BadRequest("No Tasks Present");
            }
        }

        /// <summary>
        /// Delete all tasks based on the specified filters.
        /// </summary>
        /// <param name="filters">The filters need to apply.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteAllTasks([FromQuery] Filters filters)
        {
            int userId = _userService.GetUserId();
            if (await _toDoService.RemoveAllTasks(filters,userId))
            {
                return Ok("All tasks deleted");
            }
            else
            {
                throw new Exception("something went wrong");
            }
        }

        /// <summary>
        /// Update the task based on the task Id
        /// </summary>
        /// <param name="updatedTask">he updated task details.</param>
        /// <param name="taskId">The ID of the task to update.</param>
        /// <returns> Returns the updated task </returns>
        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask([FromBody] ToDoItem updatedTask, [FromRoute] int taskId)
        {
            updatedTask.UserId = _userService.GetUserId();
            var task = await _toDoService.UpdateTask(updatedTask, taskId);
            if (task != null)
            {
                return Ok(task);
            }
            else
            {
                return BadRequest("Failed to update");
            }
        }

        /// <summary>
        /// Get the list of tasks based on status
        /// </summary>
        /// <param name="status">The status of the task to retrieve.</param>
        /// <returns> Returns the list of tasks based on status.</returns>
        /// <exception cref="Exception">Thrown when an internal error occurs.</exception>
        [HttpGet("TasksByStatus")]
        public async Task<IActionResult> GetTasksByStatus([FromQuery] bool status)
        {
            int userId = _userService.GetUserId();
            var tasks = await _toDoService.GetTasksByStatus(status,userId);
            if (tasks.Count == 0)
            {
                return NotFound("No Tasks Found");
            }
            else
            {
                return Ok(tasks);
            }
        }

        /// <summary>
        /// Updates the status of a task by its ID.
        /// </summary>
        /// <param name="taskId">The ID of the task to update.</param>
        /// <param name="patchDocument">The patch document containing the updates.</param>
        /// <returns>The updated task.</returns>
        /// <exception cref="Exception">Thrown when an internal error occurs.</exception>
        [HttpPatch("{taskId}")]
        public async Task<IActionResult> TaskCompleted([FromRoute] int taskId, JsonPatchDocument patchDocument)
        {
            int userId = _userService.GetUserId();
            var task = await _toDoService.TaskCompleted(taskId, patchDocument, userId);

            if (task != null)
            {
                return Ok(task);
            }
            else
            {
                return BadRequest("Failed to update");
            }
        }

    }
}