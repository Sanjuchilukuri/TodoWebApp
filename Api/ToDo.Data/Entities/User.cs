namespace ToDo.Data.Entities
{
    public class User : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public List<ToDoItem> ToDoItems { get; set; } = new List<ToDoItem>();

    }
}
