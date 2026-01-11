using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace UtilityManagementSystem.ViewModels
{
    // For listing bills in Index and Defaulters page
    public class BillListViewModel
    {
        public int BillID { get; set; }
        public string CustomerName { get; set; } = string.Empty; // Combined First + Last
        public string UtilityType { get; set; } = string.Empty; // e.g. Electricity
        public decimal TotalAmount { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Unpaid";
        // For the dropdowns
        public int SelectedCustomerId { get; set; }
        public int SelectedMeterId { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; }
        public IEnumerable<SelectListItem> Meters { get; set; } // Initially empty
        public List<DefaulterItem> DefaulterList { get; set; } = new List<DefaulterItem>();
    }

    // For the Detailed Invoice View
    public class BillDetailsViewModel
    {
        public int BillID { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }

        public int ReadingID { get; set; }
        public DateTime ReadingDate { get; set; }

        public DateTime BillDate { get; set; }
        public DateTime DueDate { get; set; }

        public decimal UnitsConsumed { get; set; }
        public decimal RatePerUnit { get; set; }
        public decimal TotalAmount { get; set; }

        public string Status { get; set; }

        public decimal LateFee { get; set; }
    }

    // For the Generate Bill Form
    public class GenerateBillViewModel
    {
        [Required(ErrorMessage = "Please select a reading to bill.")]
        [Display(Name = "Unbilled Meter Reading")]
        public int ReadingID { get; set; }
    }

    public class DefaulterItem
    {
        public int BillID { get; set; }
        public string CustomerName { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }
}