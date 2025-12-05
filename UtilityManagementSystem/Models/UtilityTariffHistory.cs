using System;
using System.Collections.Generic;

namespace UtilityManagementSystem.Models;

public partial class UtilityTariffHistory
{
    public int RecordId { get; set; }

    public int UtilityId { get; set; }

    public int TariffId { get; set; }

    public DateOnly? ChangedOn { get; set; }

    public int ChangedBy { get; set; }

    public virtual Employee ChangedByNavigation { get; set; } = null!;

    public virtual Tariff Tariff { get; set; } = null!;

    public virtual UtilityType Utility { get; set; } = null!;
}
