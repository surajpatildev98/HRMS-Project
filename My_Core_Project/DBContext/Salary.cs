using System;
using System.Collections.Generic;

namespace My_Core_Project.DBContext;

public partial class Salary
{
    public int SalaryId { get; set; }

    public int? EmployeeId { get; set; }

    public decimal? BasicPay { get; set; }

    public decimal? Hra { get; set; }

    public decimal? Da { get; set; }

    public decimal? Tax { get; set; }

    public decimal? NetPay { get; set; }

    public DateOnly? EffectiveFrom { get; set; }

    public virtual Employee? Employee { get; set; }
}
