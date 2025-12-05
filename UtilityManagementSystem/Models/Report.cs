using System;
using System.Collections.Generic;

namespace UtilityManagementSystem.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public string ReportType { get; set; } = null!;

    public int GeneratedBy { get; set; }

    public DateOnly? GeneratedDate { get; set; }

    public DateOnly ReportPeriodStart { get; set; }

    public DateOnly ReportPeriodEnd { get; set; }

    public string? FilePath { get; set; }

    public string? Summary { get; set; }

    public virtual Employee GeneratedByNavigation { get; set; } = null!;
}
