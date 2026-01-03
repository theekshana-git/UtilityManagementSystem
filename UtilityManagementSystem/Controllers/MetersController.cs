using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UtilityManagementSystem.Models;
using UtilityManagementSystem.Services.Interfaces;
using UtilityManagementSystem.ViewModels;

namespace UtilityManagementSystem.Controllers
{
    public class MetersController : Controller
    {
        private readonly IMetersService _metersService;
        private readonly UtilityDbContext _context;

        public MetersController(IMetersService metersService, UtilityDbContext context)
        {
            _metersService = metersService;
            _context = context;
        }

        public async Task<IActionResult> Index(int? customerId, int? utilityId)
        {
            var meters = await _context.Meters
                .Include(m => m.Customer)
                .Include(m => m.Utility)
                .Include(m => m.MeterReadings)
                .ToListAsync();

            if (customerId.HasValue)
                meters = meters.Where(m => m.CustomerId == customerId.Value).ToList();

            if (utilityId.HasValue)
                meters = meters.Where(m => m.UtilityId == utilityId.Value).ToList();

            return View(meters);
        }

        public async Task<IActionResult> Create()
        {
            var model = new MetersViewModel
            {
                CustomerList = (await _metersService.GetAllCustomersAsync())
                               .Select(c => new SelectListItem
                               {
                                   Value = c.CustomerId.ToString(),
                                   Text = c.FirstName + " " + c.LastName
                               })
                               .ToList(),

                UtilityList = (await _metersService.GetAllUtilitiesAsync())
                               .Select(u => new SelectListItem
                               {
                                   Value = u.UtilityId.ToString(),
                                   Text = u.UtilityName
                               })
                               .ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MetersViewModel model)
        {
            if (ModelState.IsValid)
            {
                var meter = new Meter
                {
                    CustomerId = model.CustomerId,
                    UtilityId = model.UtilityId,
                    Location = model.Location,
                    Status = model.Status
                };

                await _metersService.AddMeterAsync(meter);
                return RedirectToAction(nameof(Index));
            }

            model.CustomerList = (await _metersService.GetAllCustomersAsync())
                                 .Select(c => new SelectListItem
                                 {
                                     Value = c.CustomerId.ToString(),
                                     Text = c.FirstName + " " + c.LastName
                                 })
                                 .ToList();

            model.UtilityList = (await _metersService.GetAllUtilitiesAsync())
                                .Select(u => new SelectListItem
                                {
                                    Value = u.UtilityId.ToString(),
                                    Text = u.UtilityName
                                })
                                .ToList();

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var meter = await _context.Meters
                .Include(m => m.Customer)
                .Include(m => m.Utility)
                .FirstOrDefaultAsync(m => m.MeterId == id);

            if (meter == null)
                return NotFound();

            var viewModel = new MetersViewModel
            {
                MeterId = meter.MeterId,
                CustomerId = meter.CustomerId,
                UtilityId = meter.UtilityId,
                Location = meter.Location,
                Status = meter.Status,

                CustomerList = _context.Customers
                    .Select(c => new SelectListItem
                    {
                        Value = c.CustomerId.ToString(),
                        Text = c.FirstName + " " + c.LastName
                    })
                    .ToList(),

                UtilityList = _context.UtilityTypes
                    .Select(u => new SelectListItem
                    {
                        Value = u.UtilityId.ToString(),
                        Text = u.UtilityName
                    })
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MetersViewModel model)
        {
            if (id != model.MeterId) return NotFound();

            if (!ModelState.IsValid)
            {
                model.CustomerList = _context.Customers
                    .Select(c => new SelectListItem
                    {
                        Value = c.CustomerId.ToString(),
                        Text = c.FirstName + " " + c.LastName
                    })
                    .ToList();

                model.UtilityList = _context.UtilityTypes
                    .Select(u => new SelectListItem
                    {
                        Value = u.UtilityId.ToString(),
                        Text = u.UtilityName
                    })
                    .ToList();

                return View(model);
            }

            var meter = await _context.Meters.FindAsync(id);
            if (meter == null) return NotFound();

            meter.UtilityId = model.UtilityId;
            meter.Location = model.Location;
            meter.Status = model.Status;

            _context.Update(meter);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var meter = await _context.Meters.FindAsync(id);
            if (meter == null) return NotFound();

            meter.Status = meter.Status == "Active" ? "Inactive" : "Active";

            _context.Update(meter);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}