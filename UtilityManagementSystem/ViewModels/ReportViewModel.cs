using System;
using System.Collections.Generic;

namespace UtilityManagementSystem.ViewModels
{
    public class ReportViewModel
    {
        // Dashboard
        public decimal TotalMonthlyRevenue { get; set; }
        public int UnpaidBillsCount { get; set; }

        // Monthly revenue
        public int Year { get; set; } = DateTime.Now.Year;
        public int Month { get; set; } = DateTime.Now.Month;
        public decimal? Revenue { get; set; }

        // Customer usage
        public int SearchCustomerId { get; set; }
        public bool HasSearched { get; set; }
        public CustomerUsageResult? UsageSummary { get; set; }

        // Defaulters list - Updated to match SQL output
        public List<DefaulterRow> Defaulters { get; set; } = new();
    }

    public class DefaulterRow
    {
        public int CustomerID { get; set; } // Matches SQL
        public string FirstName { get; set; } = ""; // Matches SQL
        public string LastName { get; set; } = ""; // Matches SQL
        public decimal Outstanding { get; set; } // Matches SQL SUM(TotalAmount)
    }

    public class CustomerUsageResult
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public decimal TotalConsumption { get; set; }
        public decimal TotalBilled { get; set; }
        public decimal OutstandingBalance { get; set; }
    }
}