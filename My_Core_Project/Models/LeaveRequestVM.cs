using System.ComponentModel.DataAnnotations;

namespace My_Core_Project.Models
{
    public class LeaveRequestVM
    {

        public int LeaveId { get; set; }
        [Required]
        public int? EmployeeId { get; set; }
        [Required]
        public string? LeaveType { get; set; }

        [Required]
        public DateOnly? FromDate { get; set; }
        [Required]
        public DateOnly? ToDate { get; set; }

        [Required]
        public string? Reason { get; set; }
        [Required]
        public string? Status { get; set; }

        // New property for display only
        public string? EmployeeName { get; set; }
    }
}
