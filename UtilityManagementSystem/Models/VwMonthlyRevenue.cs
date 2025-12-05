using System;
using System.Collections.Generic;

namespace UtilityManagementSystem.Models;

public partial class VwMonthlyRevenue
{
    public int? Year { get; set; }

    public int? Month { get; set; }

    public decimal? TotalRevenue { get; set; }
}
