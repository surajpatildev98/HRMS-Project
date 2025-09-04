using System.ComponentModel.DataAnnotations;

namespace My_Core_Project.Models
{
    public class DesignationVM
    {
        [Required(ErrorMessage = "DesignationId is required.")]
        public int DesignationId { get; set; }

         [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = null!;


        public int? DepartmentId { get; set; }

    }
}
