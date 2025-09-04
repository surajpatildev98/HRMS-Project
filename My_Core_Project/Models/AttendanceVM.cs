namespace My_Core_Project.Models
{
    public class AttendanceVM
    {
        public int AttendanceId { get; set; }

        public int? EmployeeId { get; set; }

        public DateOnly? AttendanceDate { get; set; }

        public TimeOnly? InTime { get; set; }

        public TimeOnly? OutTime { get; set; }

        public string? Status { get; set; }

    }

}
