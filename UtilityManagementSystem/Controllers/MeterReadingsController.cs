using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UtilityManagementSystem.Services.Interfaces;
using UtilityManagementSystem.ViewModels;

namespace UtilityManagementSystem.Controllers;

public class MeterReadingsController : Controller
{
    private readonly IMeterReadingsService _service;


    private const decimal HighUsageThreshold = 100m;
    private const decimal LowUsageThreshold = 10m;
    private const int CurrentUserId = 1;

    public MeterReadingsController(IMeterReadingsService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var readings = await _service.GetReadingsHistoryAsync();
        return View(readings);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (!id.HasValue) return NotFound();

        var reading = await _service.GetReadingByIdAsync(id.Value);
        if (reading == null) return NotFound();

        return View(reading);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PopulateCustomersDropdown();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MeterReadingViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCustomersDropdown(model.CustomerId);
            return View(model);
        }


        var lastReading = await _service.GetLastReadingAsync(model.MeterId);
        decimal previous = lastReading?.CurrentReading ?? 0;

        if (model.CurrentReading < previous)
        {
            ModelState.AddModelError("CurrentReading", $"Reading must be higher than previous ({previous}).");
            await PopulateCustomersDropdown(model.CustomerId);
            return View(model);
        }


        decimal consumption = model.CurrentReading - previous;
        string statusNote = DetermineUsageStatus(consumption);


        await _service.AddReadingAsync(model, CurrentUserId, statusNote);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> UpdateNote(int id, string note)
    {
        var reading = await _service.GetReadingByIdAsync(id);
        if (reading == null) return NotFound();

        reading.Notes = note;
        await _service.UpdateReadingAsync(reading);

        return Ok(new { success = true });
    }



    [HttpGet]
    public async Task<IActionResult> GetMetersByCustomer(int customerId)
    {
        var meters = await _service.GetMetersByCustomerIdAsync(customerId);

        var result = meters.Select(m => new
        {
            id = m.MeterId,
            text = $"{m.Utility.UtilityName} - {m.Location}"
        });

        return Json(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetMeterDetails(int meterId)
    {
        var details = await _service.GetMeterFullDetailsAsync(meterId);
        return Json(details);
    }


    private async Task PopulateCustomersDropdown(int? selectedId = null)
    {
        var customers = await _service.GetAllCustomersAsync();

        var selectListItems = customers.Select(c => new
        {
            ID = c.CustomerId,
            Name = $"{c.FirstName} {c.LastName}"
        });

        ViewBag.CustomerList = new SelectList(selectListItems, "ID", "Name", selectedId);
    }


    private string DetermineUsageStatus(decimal consumption)
    {
        if (consumption > HighUsageThreshold)
        {
            return "High Usage Alert";
        }

        if (consumption < LowUsageThreshold && consumption >= 0)
        {
            return "Low Usage";
        }

        return "Normal";
    }
}