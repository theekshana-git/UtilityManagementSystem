using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using UtilityManagementSystem.Models;
using UtilityManagementSystem.ViewModels;
using UtilityManagementSystem.Services.Interfaces;

namespace UtilityManagementSystem.Services
{
    public class ReportsService : IReportsService
    {
        private readonly UtilityDbContext _context;
        private readonly ILogger<ReportsService> _logger;

        public ReportsService(UtilityDbContext context, ILogger<ReportsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ReportViewModel> GetDashboardDataAsync()
        {
            var model = new ReportViewModel();

            // 1. Calculate Monthly Revenue using C# instead of View
            var currentMonthRevenue = await _context.Payments
                .Where(p => p.PaymentDate.Year == DateTime.Now.Year && p.PaymentDate.Month == DateTime.Now.Month)
                .SumAsync(p => (decimal?)p.AmountPaid) ?? 0;

            model.TotalMonthlyRevenue = currentMonthRevenue;

            // 2. Count Unpaid bills using C# (Trusting the Status string)
            model.UnpaidBillsCount = await _context.Bills
                .CountAsync(b => b.Status != "Paid");

            return model;
        }

        public async Task<List<DefaulterRow>> GetDefaultersAsync()
        {
            // FIX: Query the Bills table directly using EF Core.
            // This bypasses the Stored Procedure and shows anyone with a status that isn't 'Paid'.
            var defaulters = await _context.Bills
                .Include(b => b.Customer)
                .Where(b => b.Status != "Paid")
                .GroupBy(b => b.Customer)
                .Select(g => new DefaulterRow
                {
                    CustomerID = g.Key.CustomerId,
                    FirstName = g.Key.FirstName,
                    LastName = g.Key.LastName,
                    // Summing the TotalAmount (which is the remaining balance due to your trigger)
                    Outstanding = g.Sum(b => b.TotalAmount)
                })
                .ToListAsync();

            return defaulters;
        }

        public async Task<decimal> GetMonthlyRevenueAsync(int year, int month)
        {
            // FIX: Query Payment table directly. 
            // This is safer than the SP because we know the table exists.
            var revenue = await _context.Payments
                .Where(p => p.PaymentDate.Year == year && p.PaymentDate.Month == month)
                .SumAsync(p => (decimal?)p.AmountPaid) ?? 0;

            return revenue;
        }

        public async Task<CustomerUsageResult?> GetCustomerUsageSummaryAsync(int customerId)
        {
            // FIX: Fetch Customer and related data directly in C#
            // This avoids the "Cartesian Join" bug in the SQL Stored Procedure.
            var customer = await _context.Customers
                .Include(c => c.Meters)
                    .ThenInclude(m => m.MeterReadings)
                .Include(c => c.Bills)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (customer == null)
            {
                // Customer does not exist
                return null;
            }

            // Calculate totals in memory
            // 1. Total Consumption: Sum of all meter readings for this customer
            var totalConsumption = customer.Meters
                .SelectMany(m => m.MeterReadings)
                .Sum(mr => mr.Consumption ?? 0);
            // Note: If Consumption is null in DB, treat as 0

            // 2. Total Billed: Sum of all bills generated
            var totalBilled = customer.Bills.Sum(b => b.TotalAmount);

            // 3. Outstanding: Sum of bills that are NOT 'Paid'
            var outstanding = customer.Bills
                .Where(b => b.Status != "Paid")
                .Sum(b => b.TotalAmount);

            return new CustomerUsageResult
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                TotalConsumption = totalConsumption,
                TotalBilled = totalBilled,
                OutstandingBalance = outstanding
            };
        }
    }
}