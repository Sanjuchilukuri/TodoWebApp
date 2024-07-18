namespace ToDo.Data.Entities
{
    public class BaseEntity
    {
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }

        public string ModifiedBy { get; set; } = string.Empty;

        public DateTime ModifiedOn { get; set; }
    }
}
