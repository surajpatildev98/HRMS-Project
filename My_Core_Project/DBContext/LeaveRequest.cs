using System;
using System.Collections.Generic;

namespace My_Core_Project.DBContext;

public partial class LeaveRequest
{
    public int LeaveId { get; set; }

    public int? EmployeeId { get; set; }

    public string? LeaveType { get; set; }

    public DateOnly? FromDate { get; set; }

    public DateOnly? ToDate { get; set; }

    public string? Reason { get; set; }

    public string? Status { get; set; }

    public virtual Employee? Employee { get; set; }
}
