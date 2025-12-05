using System;
using System.Linq;
using System.Collections.Generic;
using UtilityManagementSystem.Models;
using UtilityManagementSystem.ViewModels;
using Microsoft.EntityFrameworkCore;
using UtilityManagementSystem.Services.Interfaces;

namespace UtilityManagementSystem.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly UtilityDbContext _db;

        public DashboardService(UtilityDbContext db)
        {
            _db = db;
        }

        public DashboardViewModel GetDashboardData()
        {
            var vm = new DashboardViewModel();

            // Basic stats
            vm.TotalCustomers = _db.Customers.Count();
            vm.TotalMeters = _db.Meters.Count();
            vm.PendingBills = _db.Bills.Count(b => b.Status == "Pending");

            // Total payments this month
            vm.TotalPaymentsThisMonth =
                _db.Payments
                    .Where(p =>
                        p.PaymentDate.Year == DateTime.Now.Year &&
                        p.PaymentDate.Month == DateTime.Now.Month)
                    .Sum(p => (decimal?)p.AmountPaid) ?? 0;

            //---------------------------------------------------------
            //   FIXED: Revenue last 6 months (Group in C#, not SQL)
            //---------------------------------------------------------

            DateTime sixMonthsAgo = DateTime.Now.AddMonths(-6);

            // Get raw payments first
            var recentPayments = _db.Payments
                .Where(p => p.PaymentDate >= sixMonthsAgo)
                .OrderBy(p => p.PaymentDate)
                .ToList(); // <-- Force client-side

            // Group safely in memory
            var grouped = recentPayments
                .GroupBy(p => new { p.PaymentDate.Year, p.PaymentDate.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .ToList();

            vm.RevenueMonths = grouped
                .Select(g =>
                    new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy"))
                .ToList();

            vm.RevenueValues = grouped
                .Select(g => g.Sum(x => x.AmountPaid))
                .ToList();

            //---------------------------------------------------------
            // Last 5 payments
            //---------------------------------------------------------
            vm.RecentPayments = _db.Payments
                .OrderByDescending(p => p.PaymentDate)
                .Take(5)
                .Select(p => new RecentPaymentRow
                {
                    CustomerName = p.Customer.FirstName + " " + p.Customer.LastName,
                    Amount = p.AmountPaid,
                    Date = p.PaymentDate
                })
                .ToList();

            return vm;
        }
    }
}
