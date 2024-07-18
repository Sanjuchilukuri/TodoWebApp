using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDo.Service.DTO.ToDoItem;
using ToDo.Service.Interfaces;
using ToDo.Tests.MockData;
using ToDo.WebApi.Controllers;

namespace ToDo.Tests.Systems.Controllers
{
    public class TestTodoController
    {
        private readonly Mock<IToDoItemService> _todoService;
        private readonly Mock<IUserService> _userService;
        private readonly int _testUserId = 99;

        public TestTodoController()
        {
            _todoService = new Mock<IToDoItemService>();
            _userService = new Mock<IUserService>();
        }

        [Fact]
        public async Task AddTask_ShouldReturns201_WhenTaskIsAdded()
        {
            /// Arrange
            var todoItem = TodoMockData.GetToDoItem();
            var todoItemDetail = TodoMockData.GetToDoItemDetail();
            _todoService.Setup(_ => _.AddTask(todoItem)).ReturnsAsync(todoItemDetail);
            _userService.Setup(_ => _.GetUserId()).Returns(_testUserId);
            var sut = new ToDoItemController(_todoService.Object, _userService.Object);

            ///Act
            var result = await sut.AddTask(todoItem);

            ///Assert
            result.GetType().Should().Be(typeof(CreatedResult));
            (result as CreatedResult)?.StatusCode.Should().Be(201);
            (result as CreatedResult)?.Value.Should().Be(todoItemDetail);
        }

        [Fact]
        public async Task AddTask_ShouldReturn500Status_WhenFailedToAddTask()
        {
            /// Arrange
            var todoitem = TodoMockData.GetToDoItem();
            _todoService.Setup(_ => _.AddTask(todoitem)).ReturnsAsync((ToDoItemDetail)null!);
            _userService.Setup(_ => _.GetUserId()).Returns(_testUserId);
            var sut = new ToDoItemController(_todoService.Object, _userService.Object);

            ///Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await sut.AddTask(todoitem));
            exception.Message.Should().Be("Failed to add task");
        }

        [Fact]
        public async Task GetTaskById_ShouldReturn200Status_WhenTaskIdIsValid()
        {
            ///Arrange
            var validTaskId = TodoMockData.validTaskId;
            var todoItemDetail = TodoMockData.GetToDoItemDetail();
            _todoService.Setup(_ => _.GetTaskById(validTaskId, _testUserId)).ReturnsAsync(todoItemDetail);
            _userService.Setup(_ => _.GetUserId()).Returns(_testUserId);
            var sut = new ToDoItemController(_todoService.Object, _userService.Object);

            ///Act
            var result = await sut.GetTaskById(validTaskId);

            ///Assert
            result.GetType().Should().Be(typeof(OkObjectResult));
            (result as OkObjectResult)?.StatusCode.Should().Be(200);
            (result as OkObjectResult)?.Value.Should().Be(todoItemDetail);
        }

        [Fact]
        public async Task GetTaskById_ShouldReturn400Status_WhenTaskIdIsInValid()
        {
            ///Arrange
            var inValidTaskId = TodoMockData.inValidTaskId;
            _todoService.Setup(_ => _.GetTaskById(inValidTaskId, _testUserId)).ReturnsAsync((ToDoItemDetail)null!);
            _userService.Setup(_ => _.GetUserId()).Returns(_testUserId);
            var sut = new ToDoItemController(_todoService.Object, _userService.Object);

            ///Act
            var result = await sut.GetTaskById(inValidTaskId);

            ///Assert
            result.GetType().Should().Be(typeof(BadRequestObjectResult));
            (result as BadRequestObjectResult)?.StatusCode.Should().Be(400);
            (result as BadRequestObjectResult)?.Value.Should().Be($"Task not found with id : {inValidTaskId}");
        }

        [Fact]
        public async Task GetAllTasks_ShouldReturn200Status_WhenTasksAreAvailable()
        {
            ///Arrange
            var tasks = TodoMockData.GetTasks();
            var filters = TodoMockData.GetFilters();
            _todoService.Setup(_ => _.GetAllTasks(filters, _testUserId)).ReturnsAsync(tasks);
            _userService.Setup(_ => _.GetUserId()).Returns(_testUserId);
            var sut = new ToDoItemController(_todoService.Object, _userService.Object);
            
            ///Act
            var result = await sut.GetAllTasks(filters);
            
            ///Assert
            result.GetType().Should().Be(typeof(OkObjectResult));
            (result as OkObjectResult)?.StatusCode.Should().Be(200);
            (result as OkObjectResult)?.Value.Should().Be(tasks);
        }

        [Fact]
        public async Task GetAllTasks_ShouldReturn404Status_WhenTasksAreNotFound()
        {
            ///Arrange
            var filters = TodoMockData.GetFilters();
            _todoService.Setup(_ => _.GetAllTasks(filters, _testUserId)).ReturnsAsync(new List<ToDoItemSummary>());
            _userService.Setup(_ => _.GetUserId()).Returns(_testUserId);
            var sut = new ToDoItemController(_todoService.Object, _userService.Object);

            ///Act
            var result = await sut.GetAllTasks(filters);

            ///Assert
            result.GetType().Should().Be(typeof(NotFoundObjectResult));
            (result as NotFoundObjectResult)?.StatusCode.Should().Be(404);
            (result as NotFoundObjectResult)?.Value.Should().Be("No Tasks Found");
        }

        [Fact]
        public async Task DeleteTaskById_ShouldReturn200Status_WhenTaskIsDeleted()
        {
            ///Arrange
            var validTaskId = TodoMockData.validTaskId;
            var todoItemDetail = TodoMockData.GetToDoItemDetail();
            _todoService.Setup(_ => _.DeleteTask(validTaskId, _testUserId)).ReturnsAsync(todoItemDetail);
            _userService.Setup(_ => _.GetUserId()).Returns(_testUserId);
            var sut = new ToDoItemController(_todoService.Object, _userService.Object);
            
            ///Act
            var result = await sut.DeleteTaskById(validTaskId);
            
            ///Assert
            result.GetType().Should().Be(typeof(OkObjectResult));
            (result as OkObjectResult)?.StatusCode.Should().Be(200);
            (result as OkObjectResult)?.Value.Should().Be(todoItemDetail);
        }

        [Fact]
        public async Task DeleteTaskById_ShouldReturn400Status_WhenTaskIsNotDeleted()
        {
            ///Arrange
            var inValidTaskId = TodoMockData.inValidTaskId;
            _todoService.Setup(_ => _.DeleteTask(inValidTaskId, _testUserId)).ReturnsAsync((ToDoItemDetail)null!);
            _userService.Setup(_ => _.GetUserId()).Returns(_testUserId);
            var sut = new ToDoItemController(_todoService.Object, _userService.Object);
            
            ///Act
            var result = await sut.DeleteTaskById(inValidTaskId);

            ///Assert
            result.GetType().Should().Be(typeof(BadRequestObjectResult));
            (result as BadRequestObjectResult)?.StatusCode.Should().Be(400);
            (result as BadRequestObjectResult)?.Value.Should().Be("No Tasks Present");
        }   

        [Fact]
        public async Task UpdateTask_ShouldReturn200Status_WhenTaskIsUpdated()
        {
            ///Arrange
            var validTaskId = TodoMockData.validTaskId;
            var todoItem = TodoMockData.GetToDoItem();
            var todoItemDetail = TodoMockData.GetToDoItemDetail();
            _todoService.Setup(_ => _.UpdateTask(todoItem, validTaskId)).ReturnsAsync(todoItemDetail);
            _userService.Setup(_ => _.GetUserId()).Returns(_testUserId);
            var sut = new ToDoItemController(_todoService.Object, _userService.Object);

            ///Act
            var result = await sut.UpdateTask(todoItem, validTaskId);

            ///Assert
            result.GetType().Should().Be(typeof(OkObjectResult));
            (result as OkObjectResult)?.StatusCode.Should().Be(200);
            (result as OkObjectResult)?.Value.Should().Be(todoItemDetail);
        }

        [Fact]
        public async Task UpdateTask_ShouldReturn400Status_WhenFailedToUpdateTask()
        {
            ///Arrange
            var validTaskId = TodoMockData.validTaskId;
            var todoItem = TodoMockData.GetToDoItem();
            _todoService.Setup(_ => _.UpdateTask(todoItem, validTaskId)).ReturnsAsync((ToDoItemDetail)null!);
            _userService.Setup(_ => _.GetUserId()).Returns(_testUserId);
            var sut = new ToDoItemController(_todoService.Object, _userService.Object);
            
            ///Act
            var result = await sut.UpdateTask(todoItem, validTaskId);
            
            ///Assert
            result.GetType().Should().Be(typeof(BadRequestObjectResult));
            (result as BadRequestObjectResult)?.StatusCode.Should().Be(400);
            (result as BadRequestObjectResult)?.Value.Should().Be("Failed to update");
        }
    }
}