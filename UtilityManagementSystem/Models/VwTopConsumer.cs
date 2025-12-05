using System;
using System.Collections.Generic;

namespace UtilityManagementSystem.Models;

public partial class VwTopConsumer
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public decimal? TotalConsumption { get; set; }
}
