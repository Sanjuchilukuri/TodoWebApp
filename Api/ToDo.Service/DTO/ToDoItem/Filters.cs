namespace ToDo.Service.DTO.ToDoItem
{
    public class Filters
    {
        public string Status { get; set; } = String.Empty;

        public DateTime date { get; set; } = DateTime.Now;
    }
}