using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UtilityManagementSystem.Models;
using UtilityManagementSystem.Services.Interfaces;
using UtilityManagementSystem.ViewModels;

namespace UtilityManagementSystem.Controllers
{
    public class ComplaintsController : Controller
    {
        private readonly IComplaintsService _service;
        private readonly UtilityDbContext _context;

        public ComplaintsController(IComplaintsService service, UtilityDbContext context)
        {
            _service = service;
            _context = context;
        }

        public async Task<IActionResult> Index(string? status, int? customerId)
        {
            ViewBag.CurrentStatus = status;

            // Customers for dropdown
            var customers = await _context.Customers
                .Select(c => new { c.CustomerId, FullName = c.FirstName + " " + c.LastName })
                .ToListAsync();
            ViewBag.Customers = new SelectList(customers, "CustomerId", "FullName");
            ViewBag.CurrentCustomerId = customerId;

            // Query complaints including Utility table
            var query = _context.Complaints
                .Include(c => c.Customer)
                .Include(c => c.Utility)   // Include utility for name
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(c => c.Status == status);

            if (customerId.HasValue)
                query = query.Where(c => c.CustomerId == customerId);

            var complaints = await query
                .OrderByDescending(c => c.ComplaintDate)
                .Select(c => new ComplaintSummaryViewModel
                {
                    ComplaintId = c.ComplaintId,
                    CustomerName = c.Customer.FirstName + " " + c.Customer.LastName,
                    UtilityName = c.Utility.UtilityName, // <-- Use actual name
                    Status = c.Status,
                    ComplaintDate = c.ComplaintDate
                })
                .ToListAsync();

            return View(complaints);
        }






        public IActionResult Create()
        {
            var customers = _context.Customers
                .Select(c => new { c.CustomerId, FullName = c.FirstName + " " + c.LastName })
                .ToList();

            var utilities = _context.UtilityTypes
                .Select(u => new { u.UtilityId, u.UtilityName })
                .ToList();

            var model = new ComplaintViewModel
            {
                Customers = new SelectList(customers, "CustomerId", "FullName"),
                Utilities = new SelectList(utilities, "UtilityId", "UtilityName")
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComplaintViewModel model)
        {
            // Ensure dropdowns are repopulated if we return to the view
            model.Customers = new SelectList(_context.Customers, "CustomerId", "FullName");
            model.Utilities = new SelectList(_context.UtilityTypes, "UtilityId", "UtilityName");

            // Validate model
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Safety: default status if nothing is selected
            if (string.IsNullOrEmpty(model.Status))
            {
                model.Status = "Pending";
            }

            // Optional: ensure Status is valid to avoid CK constraint violation
            var validStatuses = new[] { "Pending", "In Progress", "Resolved" };
            if (!validStatuses.Contains(model.Status))
            {
                ModelState.AddModelError("Status", "Invalid status selected.");
                return View(model);
            }

            // Call service to create complaint
            await _service.CreateComplaintAsync(model);

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Details(int id)
        {
            var complaintEntity = await _context.Complaints
                .Include(c => c.Customer)
                .Include(c => c.Utility)
                .FirstOrDefaultAsync(c => c.ComplaintId == id);

            if (complaintEntity == null) return NotFound();

            var vm = new ComplaintViewModel
            {
                ComplaintId = complaintEntity.ComplaintId,
                CustomerId = complaintEntity.CustomerId,
                UtilityId = complaintEntity.UtilityId,
                Description = complaintEntity.Description,
                Status = complaintEntity.Status,
                ComplaintDate = complaintEntity.ComplaintDate,
                ResolutionNote = complaintEntity.ResolutionNote,
                ResolutionDate = complaintEntity.ResolutionDate,
                Employees = new SelectList(await _context.Employees
                                                .Select(e => new { e.EmployeeId, FullName = e.FirstName + " " + e.LastName })
                                                .ToListAsync(),
                                          "EmployeeId", "FullName")
            };

            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Assign(int complaintId, int employeeId)
        {
            await _service.AssignComplaintAsync(complaintId, employeeId);
            return RedirectToAction(nameof(Details), new { id = complaintId });
        }

        [HttpPost]
        public async Task<IActionResult> Resolve(int complaintId, string resolutionNote)
        {
            await _service.ResolveComplaintAsync(complaintId, resolutionNote);
            return RedirectToAction(nameof(Details), new { id = complaintId });
        }
    }
}
