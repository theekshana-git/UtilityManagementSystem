using System;
using System.Collections.Generic;

namespace UtilityManagementSystem.Models;

public partial class Meter
{
    public int MeterId { get; set; }

    public int CustomerId { get; set; }

    public int UtilityId { get; set; }

    public DateOnly InstallationDate { get; set; }

    public string Location { get; set; } = null!;

    public string? Status { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<MeterReading> MeterReadings { get; set; } = new List<MeterReading>();

    public virtual UtilityType Utility { get; set; } = null!;
}
