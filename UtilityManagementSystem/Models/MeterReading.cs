using System;
using System.Collections.Generic;

namespace UtilityManagementSystem.Models;

public partial class MeterReading
{
    public int ReadingId { get; set; }

    public int MeterId { get; set; }

    public DateOnly ReadingDate { get; set; }

    public decimal? PreviousReading { get; set; }

    public decimal CurrentReading { get; set; }

    public decimal? Consumption { get; set; }

    public int RecordedBy { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual Meter Meter { get; set; } = null!;

    public virtual Employee RecordedByNavigation { get; set; } = null!;
}
