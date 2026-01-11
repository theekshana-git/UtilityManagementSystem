using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UtilityManagementSystem.Models;

namespace UtilityManagementSystem.ViewModels
{
    public class PaymentsViewModel
    {
        public int BillId { get; set; }
        public int CustomerId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public decimal OutstandingAmount { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal AmountPaid { get; set; }

        public string PaymentMethod { get; set; } = "Cash";

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        // READ-ONLY source for dropdown
        public IEnumerable<VwUnpaidBill> UnpaidBills { get; set; } = new List<VwUnpaidBill>();
    }

   
}
