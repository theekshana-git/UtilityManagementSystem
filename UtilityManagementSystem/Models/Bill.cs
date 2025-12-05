using System;
using System.Collections.Generic;

namespace UtilityManagementSystem.Models;

public partial class Bill
{
    public int BillId { get; set; }

    public int CustomerId { get; set; }

    public int ReadingId { get; set; }

    public DateOnly? BillDate { get; set; }

    public DateOnly DueDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Status { get; set; }

    public int GeneratedBy { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Employee GeneratedByNavigation { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual MeterReading Reading { get; set; } = null!;
}
