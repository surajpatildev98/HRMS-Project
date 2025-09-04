using System.ComponentModel.DataAnnotations;

namespace My_Core_Project.Models
{
    public class DepartmentVM
    {
        [Required(ErrorMessage = "DepartmentId is required.")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        public string DepartmentName { get; set; } = null!;
    }
}
