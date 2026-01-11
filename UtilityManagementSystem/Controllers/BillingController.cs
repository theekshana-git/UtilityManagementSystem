using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UtilityManagementSystem.Services.Interfaces;
using UtilityManagementSystem.ViewModels;
using UtilityManagementSystem.Models;

namespace UtilityManagementSystem.Controllers
{
    public class BillingController : Controller
    {
        private readonly IBillingService _billingService;
        private readonly UtilityDbContext _context;

        public BillingController(IBillingService billingService, UtilityDbContext context)
        {
            _billingService = billingService;
            _context = context;
        }

        public async Task<IActionResult> Index(string status, string search)
        {
            var bills = await _billingService.GetAllBillsAsync(status, search);
            ViewBag.CurrentStatus = status;
            ViewBag.CurrentSearch = search;
            return View(bills);
        }

        public async Task<IActionResult> Details(int id)
        {
            var bill = await _billingService.GetBillDetailsAsync(id);
            if (bill == null) return NotFound();
            return View(bill);
        }

        // GET: Billing/Create
        public async Task<IActionResult> Create()
        {
            var customers = await _billingService.GetCustomersAsync();
            ViewBag.Customers = new SelectList(customers, "CustomerId", "FirstName");
            return View(new GenerateBillViewModel());
        }

        // POST: Billing/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GenerateBillViewModel model)
        {
            if (ModelState.IsValid)
            {
                // In a real app, get the ID from User.Identity. 1 is used as placeholder.
                await _billingService.GenerateBillAsync(model.ReadingID, 1);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // AJAX: Billing/GetMetersByCustomer
        [HttpGet]
        public async Task<IActionResult> GetMetersByCustomer(int customerId)
        {
            var meters = await _billingService.GetMetersByCustomerIdAsync(customerId);
            var result = meters.Select(m => new { value = m.MeterId, text = $"Meter #{m.MeterId} ({m.Location})" });
            return Json(result);
        }

        // AJAX: Billing/GetUnbilledReading
        [HttpGet]
        public async Task<IActionResult> GetUnbilledReading(int meterId)
        {
            var reading = await _billingService.GetLatestUnbilledReadingAsync(meterId);
            if (reading == null) return Json(null);
            return Json(new { readingId = reading.ReadingId });
        }

        // GET: Billing/Defaulters
        public IActionResult Defaulters()
        {
            // Capture the current date for comparison
            var today = DateOnly.FromDateTime(DateTime.Now);

            // Fetch only Unpaid bills that are past their due date
            var unpaidBills = _context.Bills
                .Include(b => b.Customer)
                .Where(b => b.Status == "Unpaid" && b.DueDate < today)
                .AsEnumerable() // Switch to memory for date conversion
                .Select(b => new DefaulterItem
                {
                    BillID = b.BillId,
                    CustomerName = b.Customer.FirstName + " " + b.Customer.LastName,
                    DueDate = b.DueDate.ToDateTime(TimeOnly.MinValue),
                    TotalAmount = b.TotalAmount,
                    Status = b.Status
                }).ToList();

            // Wrap the list in the ViewModel as required by the View
            var viewModel = new BillListViewModel
            {
                DefaulterList = unpaidBills
            };

            return View(viewModel);
        }
    }
}