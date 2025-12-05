using System;
using System.Collections.Generic;

namespace UtilityManagementSystem.Models;

public partial class Complaint
{
    public int ComplaintId { get; set; }

    public int CustomerId { get; set; }

    public int UtilityId { get; set; }

    public string Description { get; set; } = null!;

    public DateOnly? ComplaintDate { get; set; }

    public string? Status { get; set; }

    public int? HandledBy { get; set; }

    public string? ResolutionNote { get; set; }

    public DateOnly? ResolutionDate { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Employee? HandledByNavigation { get; set; }

    public virtual UtilityType Utility { get; set; } = null!;
}
