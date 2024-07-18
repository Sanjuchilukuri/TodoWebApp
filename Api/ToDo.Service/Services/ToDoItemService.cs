using AutoMapper;
using ToDo.Service.DTO.ToDoItem;
using ToDo.Service.Interfaces;
using ToDo.Data.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Http;

namespace ToDo.Service.Services
{
    /// <inheritdoc cref="IToDoItemService"/>
    public class ToDoItemService : IToDoItemService
    {
        private IToDoItemRepo _ToDoItemRepo;
        private IMapper _mapper;

        /// <summary>
        /// Constructor for <see cref="ToDoItemServices"/>
        /// </summary>
        /// <param name="toDoItemRepo"><see cref="IToDoItemRepo"/></param>
        /// <param name="mapper"><see cref="IMapper"/></param>
        /// <param name="httpContextAccessor"><see cref="IHttpContextAccessor"/></param>
        public ToDoItemService(IToDoItemRepo toDoItemRepo, IMapper mapper)
        {
            _ToDoItemRepo = toDoItemRepo;
            _mapper = mapper;
        }

        public async Task<ToDoItemDetail> AddTask(DTO.ToDoItem.ToDoItem newItem)
        {
            return _mapper.Map<ToDoItemDetail>(await _ToDoItemRepo.AddTaskAsync(_mapper.Map<Data.Entities.ToDoItem>(newItem)));
        }

        public async Task<ToDoItemDetail> DeleteTask(int taskId, int userId)
        {
            return _mapper.Map<ToDoItemDetail>(await _ToDoItemRepo.RemoveTaskAsync(taskId, userId));
        }

        public async Task<List<ToDoItemSummary>> GetAllTasks(Filters filters, int userId)
        {
            List<ToDoItemSummary> todoItems = new List<ToDoItemSummary>();

            foreach (var item in await _ToDoItemRepo.GetAllTasksAsync(userId, filters.Status, filters.date))
            {
                todoItems.Add(_mapper.Map<ToDoItemSummary>(item));
            }

            return todoItems;
        }

        public async Task<ToDoItemDetail> GetTaskById(int Taskid, int userId)
        {
            return _mapper.Map<ToDoItemDetail>(await _ToDoItemRepo.GetTaskByIdAsync(Taskid, userId));
        }

        public Task<bool> RemoveAllTasks(Filters filters, int userId)
        {
            return _ToDoItemRepo.RemoveAllTasksAsync(userId, filters.Status, filters.date);
        }

        public async Task<ToDoItemDetail> UpdateTask(DTO.ToDoItem.ToDoItem item, int taskId)
        {
            var task = _mapper.Map<Data.Entities.ToDoItem>(item);
            task.Id = taskId;
            return _mapper.Map<ToDoItemDetail>(await _ToDoItemRepo.UpdateTaskAsync(task));
        }

        public async Task<ToDoItemDetail> TaskCompleted(int taskId, JsonPatchDocument patchDocument, int userId)
        {
            var task = await _ToDoItemRepo.GetTaskByIdAsync(taskId, userId);
            if (task == null)
            {
                return null!;
            }
            patchDocument.ApplyTo(task);
            return _mapper.Map<ToDoItemDetail>(await _ToDoItemRepo.UpdateTaskAsync(task));
        }

        public async Task<List<ToDoItemDetail>> GetTasksByStatus(bool status, int userId)
        {
            List<ToDoItemDetail> todoActiveItems = new List<ToDoItemDetail>();

            foreach (var item in await _ToDoItemRepo.GetTasksByStatusAsync(userId, status))
            {
                todoActiveItems.Add(_mapper.Map<ToDoItemDetail>(item));
            }

            return todoActiveItems;
        }
    }
}