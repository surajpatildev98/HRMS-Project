using System.ComponentModel.DataAnnotations;

namespace My_Core_Project.Models
{
    using System.ComponentModel.DataAnnotations;

    public class EmployeeVM
    {
        [Required(ErrorMessage = "EmployeeId is required.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name must be under 50 characters.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name must be under 50 characters.")]
        public string? LastName { get; set; }

        [StringLength(50, ErrorMessage = "Email must be under 50 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(20, ErrorMessage = "Phone number must be under 20 characters.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(1, ErrorMessage = "Gender must be 1 character (M/F/O).")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateOnly? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Join date is required.")]
        public DateOnly? JoinDate { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public int? DepartmentId { get; set; }
       
        [Required(ErrorMessage = "Designation is required.")]
        public int? DesignationId { get; set; }

        [Required(ErrorMessage = "Employment type is required.")]
        [StringLength(20, ErrorMessage = "Employment type must be under 20 characters.")]
        public string? EmploymentType { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Status must be under 20 characters.")]
        public string? Status { get; set; }

        public byte[]? ProfileImage { get; set; }
#nullable disable
        public string? ProfileImageBase64 => ProfileImage != null ? Convert.ToBase64String(ProfileImage) : null;
#nullable restore
        public string? DepartmentName { get; set; }  // For displaying
        public string? DesignationName { get; set; } // For displaying
    }

}
