using Microsoft.AspNetCore.Mvc;
using System.Text;
using UtilityManagementSystem.Services.Interfaces;
using UtilityManagementSystem.ViewModels;

namespace UtilityManagementSystem.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportsService _reportService;

        public ReportsController(IReportsService reportService)
        {
            _reportService = reportService;
        }

        public async Task<IActionResult> Index()
        {
            // Fetches the dashboard stats for the quick cards
            var model = await _reportService.GetDashboardDataAsync();
            return View(model);
        }

        public async Task<IActionResult> Defaulters()
        {
            var model = new ReportViewModel
            {
                Defaulters = await _reportService.GetDefaultersAsync()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult MonthlyRevenue()
        {
            return View(new ReportViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> MonthlyRevenue(ReportViewModel model)
        {
            if (model.Year == 0) model.Year = DateTime.Now.Year;
            if (model.Month == 0) model.Month = DateTime.Now.Month;

            model.Revenue = await _reportService.GetMonthlyRevenueAsync(model.Year, model.Month);
            return View(model);
        }

        [HttpGet]
        public IActionResult CustomerUsage()
        {
            return View(new ReportViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CustomerUsage(ReportViewModel model)
        {
            model.HasSearched = true; // 🔥 IMPORTANT
            model.UsageSummary = await _reportService
                .GetCustomerUsageSummaryAsync(model.SearchCustomerId);

            return View(model);
        }

        public async Task<IActionResult> ExportDefaulters()
        {
            var data = await _reportService.GetDefaultersAsync();

            var csv = new StringBuilder();
            // Header Row
            csv.AppendLine("Customer ID,First Name,Last Name,Outstanding Balance");

            foreach (var item in data)
            {
                csv.AppendLine($"{item.CustomerID},{item.FirstName},{item.LastName},{item.Outstanding}");
            }

            byte[] buffer = Encoding.UTF8.GetBytes(csv.ToString());
            return File(buffer, "text/csv", $"Defaulters_{DateTime.Now:yyyyMMdd}.csv");
        }
    }
}
