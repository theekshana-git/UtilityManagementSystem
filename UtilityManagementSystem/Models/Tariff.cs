using System;
using System.Collections.Generic;

namespace UtilityManagementSystem.Models;

public partial class Tariff
{
    public int TariffId { get; set; }

    public int UtilityId { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }

    public decimal? FixedCharge { get; set; }

    public decimal SlabStart { get; set; }

    public decimal? SlabEnd { get; set; }

    public decimal RatePerUnit { get; set; }

    public virtual UtilityType Utility { get; set; } = null!;

    public virtual ICollection<UtilityTariffHistory> UtilityTariffHistories { get; set; } = new List<UtilityTariffHistory>();
}
