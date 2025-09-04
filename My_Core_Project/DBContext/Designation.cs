using System;
using System.Collections.Generic;

namespace My_Core_Project.DBContext;

public partial class Designation
{
    public int DesignationId { get; set; }

    public string Title { get; set; } = null!;

    public int? DepartmentId { get; set; }

    public virtual ICollection<Appraisal> Appraisals { get; set; } = new List<Appraisal>();

    public virtual Department? Department { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
