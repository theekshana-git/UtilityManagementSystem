using System;
using System.Collections.Generic;

namespace UtilityManagementSystem.Models;

public partial class UtilityType
{
    public int UtilityId { get; set; }

    public string UtilityName { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    public virtual ICollection<Meter> Meters { get; set; } = new List<Meter>();

    public virtual ICollection<Tariff> Tariffs { get; set; } = new List<Tariff>();

    public virtual ICollection<UtilityTariffHistory> UtilityTariffHistories { get; set; } = new List<UtilityTariffHistory>();
}
