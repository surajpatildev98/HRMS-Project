using System;
using System.Collections.Generic;

namespace My_Core_Project.DBContext;

public partial class Appraisal
{
    public int AppraisalId { get; set; }

    public int EmployeeId { get; set; }

    public DateOnly AppraisalDate { get; set; }

    public decimal? PreviousCtc { get; set; }

    public decimal? NewCtc { get; set; }

    public int? PromotedToDesignationId { get; set; }

    public int? AppraiserId { get; set; }

    public string? Comments { get; set; }

    public virtual Employee? Appraiser { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Designation? PromotedToDesignation { get; set; }
}
