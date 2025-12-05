using System;
using System.Collections.Generic;

namespace UtilityManagementSystem.Models;

public partial class VwComplaintsSummary
{
    public int ComplaintId { get; set; }

    public string CustomerName { get; set; } = null!;

    public int UtilityId { get; set; }

    public string? Status { get; set; }

    public DateOnly? ComplaintDate { get; set; }
}
