using System;
using System.Collections.Generic;

namespace UtilityManagementSystem.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int BillId { get; set; }

    public int CustomerId { get; set; }

    public DateTime PaymentDate { get; set; }

    public decimal AmountPaid { get; set; }

    public string? PaymentMethod { get; set; }

    public int ProcessedBy { get; set; }

    public string? ReferenceNumber { get; set; }

    public virtual Bill Bill { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual Employee ProcessedByNavigation { get; set; } = null!;
}
