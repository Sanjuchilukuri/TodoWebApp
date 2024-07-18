using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ToDo.Service.DTO.ToDoItem
{
    public class ToDoItem
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [JsonIgnore]
        public int UserId { get; set; }

    }
}
