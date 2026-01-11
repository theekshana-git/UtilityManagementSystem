using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Added for [Key]
using System.ComponentModel.DataAnnotations.Schema; // Added for [Column] and [Table]

namespace UtilityManagementSystem.Models;

// Ensuring the table name matches what the database expects
[Table("Bill")]
public partial class Bill
{
    // Fixes the "Required column 'BillID' was not present" error
    [Key]
    [Column("BillID")]
    public int BillId { get; set; }

    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [Column("ReadingID")]
    public int ReadingId { get; set; }

    public DateOnly BillDate { get; set; }

    public DateOnly DueDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Status { get; set; }

    public int GeneratedBy { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Employee GeneratedByNavigation { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual MeterReading Reading { get; set; } = null!;
}