using ToDo.Service.DTO.ToDoItem;

namespace ToDo.Tests.MockData
{
    public class TodoMockData
    {
        public static int validTaskId = 1;

        public static int inValidTaskId = 0;

        public static ToDoItem GetToDoItem()
        {
            return new ToDoItem
            {
                Title = "Test Task",
                Description = "Test Description",
                UserId = 1
            };
        }

        public static ToDoItemDetail GetToDoItemDetail()
        {
            return new ToDoItemDetail
            {
                Title = "Test Task",
                Description = "Test Description",
                IsCompleted = false,
                CreatedOn = DateTime.Now
            };
        }

        public static Filters GetFilters()
        {
            return new Filters
            {
                Status = "All",
                date = DateTime.Now
            };
        }

        public static List<ToDoItemSummary> GetTasks()
        {
            return new List<ToDoItemSummary>
            {
                new ToDoItemSummary
                {
                    Title = "Test Task",
                    IsCompleted = false
                },
                new ToDoItemSummary
                {
                    Title = "Test Task 2",
                    IsCompleted = true
                }
            };
        }
        

    }
}
