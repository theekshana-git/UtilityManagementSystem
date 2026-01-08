using System.Collections.Generic;

namespace UtilityManagementSystem.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalCustomers { get; set; }
        public int TotalMeters { get; set; }
        public decimal TotalPaymentsThisMonth { get; set; }
        public int PendingBills { get; set; }

        // Chart: Revenue per month
        public List<string> RevenueMonths { get; set; }
        public List<decimal> RevenueValues { get; set; }

        // Table: Last 5 payments
        public List<RecentPaymentRow> RecentPayments { get; set; }
    }

    public class RecentPaymentRow
    {
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}