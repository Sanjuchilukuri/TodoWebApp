using System.ComponentModel.DataAnnotations;

namespace ToDo.Service.DTO.User
{
    public class LoginRegisterModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}