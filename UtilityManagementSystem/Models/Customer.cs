using System;
using System.Collections.Generic;

namespace UtilityManagementSystem.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? CustomerType { get; set; }

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public string? Email { get; set; }

    public DateOnly? RegistrationDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    public virtual ICollection<Meter> Meters { get; set; } = new List<Meter>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
