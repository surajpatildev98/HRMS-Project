using System.ComponentModel.DataAnnotations;

namespace My_Core_Project.Models
{
    public class UserVM
    {
       
        public int UserId { get; set; }

        public int? EmployeeId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; } = null!;

        [Required(ErrorMessage = "Status is required.")]
        public bool? IsActive { get; set; }

        public bool? IsAdmin { get; set; }
    }
}
