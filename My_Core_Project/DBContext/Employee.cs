using System;
using System.Collections.Generic;

namespace My_Core_Project.DBContext;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public DateOnly? JoinDate { get; set; }

    public int? DepartmentId { get; set; }

    public int? DesignationId { get; set; }


    public string? EmploymentType { get; set; }

    public string? Status { get; set; }

    public byte[]? ProfileImage { get; set; }

    public virtual ICollection<Appraisal> AppraisalAppraisers { get; set; } = new List<Appraisal>();

    public virtual ICollection<Appraisal> AppraisalEmployees { get; set; } = new List<Appraisal>();

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Department? Department { get; set; }

    public virtual Designation? Designation { get; set; }

    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    public virtual ICollection<PerformanceReview> PerformanceReviewEmployees { get; set; } = new List<PerformanceReview>();

    public virtual ICollection<PerformanceReview> PerformanceReviewReviewers { get; set; } = new List<PerformanceReview>();

    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

    public virtual User? User { get; set; }
}
