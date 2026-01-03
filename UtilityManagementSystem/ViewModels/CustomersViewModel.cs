using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UtilityManagementSystem.ViewModels
{
    public class CustomersViewModel
    {
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Customer Type is required")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [Phone(ErrorMessage = "Invalid contact number")]//if number exceeds 10 digits
        public string Contact { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Register date is required")]
        public DateTime Register { get; set; }

        public decimal Outstanding { get; set; }

        public bool IsActive { get; set; }

        public List<MeterViewModel> Meters { get; set; } = new();
        public List<BillViewModel> UnpaidBills { get; set; } = new();
        public List<PaymentViewModel> Payments { get; set; } = new();

        public CustomerUsageViewModel? UsageSummary { get; set; }
    }

    public class MeterViewModel
    {
        public int MeterID { get; set; }
        public string MeterNumber { get; set; }
        public DateTime InstallationDate { get; set; }
    }

    public class BillViewModel
    {
        public int BillID { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class PaymentViewModel
    {
        public int PaymentID { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

    public class CustomerUsageViewModel
    {
        public decimal TotalUsage { get; set; }
        public decimal AvgUsagePerMonth { get; set; }
    }

    public class CustomerUsageDto
    {
        public decimal TotalUsage { get; set; }
        public decimal AvgUsagePerMonth { get; set; }
    }
}