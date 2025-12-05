using System;
using System.Collections.Generic;

namespace UtilityManagementSystem.Models;

public partial class VwUnpaidBill
{
    public int BillId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public DateOnly DueDate { get; set; }
}
