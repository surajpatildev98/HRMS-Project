using System;
using System.Collections.Generic;

namespace My_Core_Project.DBContext;

public partial class PerformanceReview
{
    public int ReviewId { get; set; }

    public int? EmployeeId { get; set; }

    public int? ReviewerId { get; set; }

    public DateOnly? ReviewDate { get; set; }

    public string? Comments { get; set; }

    public int? Rating { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Employee? Reviewer { get; set; }
}
