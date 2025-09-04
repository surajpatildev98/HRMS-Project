using System;
using System.Collections.Generic;

namespace My_Core_Project.DBContext;

public partial class AuditLog
{
    public int LogId { get; set; }

    public int? UserId { get; set; }

    public string? Action { get; set; }

    public string? TableAffected { get; set; }

    public DateTime? Timestamp { get; set; }

    public string? Ipaddress { get; set; }

    public virtual User? User { get; set; }
}
